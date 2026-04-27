import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip, finalize } from 'rxjs/operators';
import { CuentaService, CuentaDTO } from '../../../services/cuenta.service';
import { LanguageService } from '../../../services/language.service';
import { TranslationService } from '../../../services/translation.service';
import { SystemConfigurationService, SystemConfigurationDTO } from '../../../services/system-configuration.service';
import { PaisService, PaisDTO } from '../../../services/pais.service';
import { ProvinciaService, ProvinciaDTO } from '../../../services/provincia.service';
import { MunicipioService, MunicipioDTO } from '../../../services/municipio.service';
import { CodigoPostalService, CodigoPostalDTO } from '../../../services/codigo-postal.service';
import { FormaJuridicaService } from '../../../services/forma-juridica.service';
import { normalizePhone, phoneValidator, PHONE_MAX_LENGTH } from '../../shared/phone.util';
import { 
  TipoIdentificacionFiscal, 
  RegimenFiscal,
  TIPO_IDENTIFICACION_FISCAL_OPTIONS,
  REGIMEN_FISCAL_OPTIONS 
} from '../../../services/fiscal.types';

@Component({
  selector: 'app-system-configuration-form',
  templateUrl: './system-configuration-form.component.html',
  styleUrls: ['./system-configuration-form.component.css']
})
export class SystemConfigurationFormComponent implements OnInit, OnDestroy {
  form: FormGroup;
  isEditMode = false;
  configId: number | null = null;
  loading = false;
  saving = false;
  error: string | null = null;
  cuentas: CuentaDTO[] = [];
  loadingCuentas = true;

  selectedLogoFile: File | null = null;
  logoPreviewUrl: string | null = null;
  selectedPais?: PaisDTO;

  paises: PaisDTO[] = [];
  provincias: ProvinciaDTO[] = [];
  municipios: MunicipioDTO[] = [];
  codigosPostales: CodigoPostalDTO[] = [];
  formasJuridicas: any[] = [];

  tiposIdentificacionFiscal = TIPO_IDENTIFICACION_FISCAL_OPTIONS;
  regimenesFiscales = REGIMEN_FISCAL_OPTIONS;

  private languageSubscription?: Subscription;
  private paisSub?: Subscription;
  private provinciaSub?: Subscription;
  private municipioSub?: Subscription;
  private paisDetailSub?: Subscription;

  constructor(
    private fb: FormBuilder,
    private systemConfigurationService: SystemConfigurationService,
    private cuentaService: CuentaService,
    private formaJuridicaService: FormaJuridicaService,
    private router: Router,
    private route: ActivatedRoute,
    private languageService: LanguageService,
    public translationService: TranslationService,
    private paisService: PaisService,
    private provinciaService: ProvinciaService,
    private municipioService: MunicipioService,
    private codigoPostalService: CodigoPostalService
  ) {
    this.form = this.fb.group({
      codigoSistema: ['', [
        Validators.required, 
        Validators.minLength(3), 
        Validators.maxLength(3),
        Validators.pattern(/^[A-Za-z0-9]{3}$/)
      ]],
      licencia: ['', [Validators.required, Validators.maxLength(400)]],
      nombreNegocio: ['', [Validators.required, Validators.maxLength(256)]],
      personaContacto: ['', [Validators.required, Validators.maxLength(256)]],
      formaJuridicaId: [null, Validators.required],
      direccion: ['', Validators.maxLength(512)],
      paisId: [null, Validators.required],
      provinciaId: [{ value: null, disabled: true }, Validators.required],
      municipioId: [{ value: null, disabled: true }, Validators.required],
      codigoPostalId: [{ value: null, disabled: true }, Validators.required],
      imagen: ['', Validators.maxLength(500)],
      web: ['', Validators.maxLength(256)],
      email: ['', [Validators.email, Validators.maxLength(256)]],
      telefono: ['', Validators.maxLength(PHONE_MAX_LENGTH)],
      cuentaPagarId: [{ value: null, disabled: true }, Validators.required],
      cuentaCobrarId: [{ value: null, disabled: true }, Validators.required],
      caducidad: ['', Validators.required],
      activo: [true],
      
      identificadorFiscal: ['', Validators.maxLength(30)],
      tipoIdentificadorFiscal: [null, Validators.required],
      regimenFiscal: [null, Validators.required],
      tasaIvaDefecto: [21, [
        Validators.required, 
        Validators.min(0), 
        Validators.max(99.99),
        Validators.pattern(/^\d{1,2}(\.\d{1,2})?$/)
      ]],
      registradaIva: [true],
      ivaInternacional: [false]
    });
  }

  ngOnInit(): void {
    this.setupFormSubscriptions();
    this.loadFormasJuridicas();
    this.loadPaises();
    this.loadCuentas();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.configId = +id;
      this.isEditMode = true;
      this.loadConfiguration();
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        this.loadCuentas();
        this.loadPaises();
        this.loadFormasJuridicas();
        if (this.isEditMode) this.loadConfiguration();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
    this.paisSub?.unsubscribe();
    this.provinciaSub?.unsubscribe();
    this.municipioSub?.unsubscribe();
    this.paisDetailSub?.unsubscribe();
  }

  private setupFormSubscriptions(): void {
    const upperFields = ['codigoSistema', 'nombreNegocio', 'direccion'];
    upperFields.forEach(field => {
      this.form.get(field)?.valueChanges.subscribe(value => {
        if (value && typeof value === 'string' && value !== value.toUpperCase()) {
          this.form.get(field)?.setValue(value.toUpperCase(), { emitEvent: false });
        }
      });
    });

    this.paisSub = this.form.get('paisId')?.valueChanges.subscribe((paisIdRaw: any) => {
      const paisId = paisIdRaw ? Number(paisIdRaw) : 0;
      const provControl = this.form.get('provinciaId');
      
      this.provincias = [];
      this.municipios = [];
      this.codigosPostales = [];
      this.form.patchValue({ provinciaId: null, municipioId: null, codigoPostalId: null }, { emitEvent: false });

      this.paisDetailSub?.unsubscribe();
      this.selectedPais = undefined;
      this.applyPhoneValidators(undefined);

      if (paisId) {
        provControl?.enable({ emitEvent: false });
        this.loadProvincias(paisId);
        
        this.paisDetailSub = this.paisService.getById(paisId).subscribe({
          next: p => this.applyPhoneValidators(p),
          error: () => this.applyPhoneValidators(undefined)
        });
      } else {
        provControl?.disable({ emitEvent: false });
      }
    });

    this.provinciaSub = this.form.get('provinciaId')?.valueChanges.subscribe((provRaw: any) => {
      const provinciaId = provRaw ? Number(provRaw) : 0;
      const munControl = this.form.get('municipioId');
      
      this.municipios = [];
      this.codigosPostales = [];
      this.form.patchValue({ municipioId: null, codigoPostalId: null }, { emitEvent: false });

      if (provinciaId) {
        munControl?.enable({ emitEvent: false });
        this.loadMunicipios(provinciaId);
      } else {
        munControl?.disable({ emitEvent: false });
      }
    });

    this.municipioSub = this.form.get('municipioId')?.valueChanges.subscribe((munRaw: any) => {
      const municipioId = munRaw ? Number(munRaw) : 0;
      const cpControl = this.form.get('codigoPostalId');
      
      this.codigosPostales = [];
      this.form.patchValue({ codigoPostalId: null }, { emitEvent: false });

      if (municipioId) {
        cpControl?.enable({ emitEvent: false });
        this.loadCodigosPostales(municipioId);
      } else {
        cpControl?.disable({ emitEvent: false });
      }
    });
  }

  private applyPhoneValidators(pais?: PaisDTO): void {
    this.selectedPais = pais;
    const telefono = this.form.get('telefono')!;
    
    const validators = [Validators.maxLength(PHONE_MAX_LENGTH)];
    if (pais && pais.regexTelefono) {
      validators.push(phoneValidator(pais.regexTelefono));
    }
    
    telefono.setValidators(validators);
    telefono.updateValueAndValidity({ emitEvent: true });
  }

  private loadFormasJuridicas(): void {
    const lang = this.languageService.getCurrentLanguage();
    this.systemConfigurationService.getFormasJuridicas(lang).subscribe({
      next: (data: any) => this.formasJuridicas = data,
      error: (err: any) => console.error('Error loading formas juridicas:', err)
    });
  }

  private loadPaises(): void {
    this.systemConfigurationService.getPaises().subscribe({
      next: (data: any) => this.paises = data,
      error: (err: any) => console.error('Error loading paises:', err)
    });
  }

  private loadCuentas(): void {
    this.loadingCuentas = true;
    
    this.cuentaService.getAll().subscribe({
      next: (data: CuentaDTO[]) => {
        this.cuentas = data.filter(c => !c.cancelado);
        
        const pagarControl = this.form.get('cuentaPagarId');
        const cobrarControl = this.form.get('cuentaCobrarId');
        
        if (pagarControl) {
          pagarControl.setValidators([Validators.required]);
          pagarControl.updateValueAndValidity({ emitEvent: false });
          pagarControl.enable({ emitEvent: false });
        }
        
        if (cobrarControl) {
          cobrarControl.setValidators([Validators.required]);
          cobrarControl.updateValueAndValidity({ emitEvent: false });
          cobrarControl.enable({ emitEvent: false });
        }
        
        this.loadingCuentas = false;
      },
      error: (err: any) => {
        console.error('Error loading cuentas', err);
        this.form.get('cuentaPagarId')?.enable({ emitEvent: false });
        this.form.get('cuentaCobrarId')?.enable({ emitEvent: false });
        this.loadingCuentas = false;
      }
    });
  }

  private loadProvincias(paisId: number): void {
    this.provinciaService.getByPaisId(paisId).subscribe({
      next: (data: any) => this.provincias = data,
      error: (err: any) => console.error('Error loading provincias:', err)
    });
  }

  private loadMunicipios(provinciaId: number): void {
    this.municipioService.getByProvinciaId(provinciaId).subscribe({
      next: (data: any) => this.municipios = data,
      error: (err: any) => console.error('Error loading municipios:', err)
    });
  }

  private loadCodigosPostales(municipioId: number): void {
    this.codigoPostalService.getByMunicipioId(municipioId).subscribe({
      next: (data: any) => this.codigosPostales = data,
      error: (err: any) => console.error('Error loading codigos postales:', err)
    });
  }

  private loadConfiguration(): void {
    if (!this.configId) return;
    
    this.loading = true;
    this.systemConfigurationService.getById(this.configId).subscribe({
      next: (data: SystemConfigurationDTO) => {
        const formattedCaducidad = data.caducidad
          ? new Date(data.caducidad).toISOString().substring(0, 10)
          : '';

        this.form.patchValue({
          codigoSistema: data.codigoSistema,
          licencia: data.licencia,
          nombreNegocio: data.nombreNegocio,
          personaContacto: data.personaContacto ?? '',
          formaJuridicaId: data.formaJuridicaId ?? null,
          direccion: data.direccion,
          imagen: data.imagen,
          web: data.web,
          email: data.email,
          telefono: data.telefono,
          cuentaPagarId: data.cuentaPagarId ? Number(data.cuentaPagarId) : null,
          cuentaCobrarId: data.cuentaCobrarId ? Number(data.cuentaCobrarId) : null,
          caducidad: formattedCaducidad,
          activo: data.activo ?? true,
          identificadorFiscal: data.identificadorFiscal ?? '',
          tipoIdentificadorFiscal: data.tipoIdentificadorFiscal ?? TipoIdentificacionFiscal.NIF,
          regimenFiscal: data.regimenFiscal ?? RegimenFiscal.General,
          tasaIvaDefecto: data.tasaIvaDefecto ?? 21,
          registradaIva: data.registradaIva ?? true,
          ivaInternacional: data.ivaInternacional ?? false
        }, { emitEvent: false });

        const paisId = data.paisId ?? 0;
        const provinciaId = data.provinciaId ?? 0;
        const municipioId = data.municipioId ?? 0;
        const codigoPostalId = data.codigoPostalId ?? 0;

        if (paisId) {
          this.form.patchValue({ paisId: paisId }, { emitEvent: false });
          
          this.paisDetailSub?.unsubscribe();
          this.paisDetailSub = this.paisService.getById(paisId).subscribe({
            next: p => this.applyPhoneValidators(p),
            error: () => this.applyPhoneValidators(undefined)
          });

          this.provinciaService.getByPaisId(paisId).subscribe({
            next: provinces => {
              this.provincias = provinces;
              this.form.get('provinciaId')?.enable({ emitEvent: false });

              if (provinciaId) {
                this.form.patchValue({ provinciaId: provinciaId }, { emitEvent: false });

                this.municipioService.getByProvinciaId(provinciaId).subscribe({
                  next: municipios => {
                    this.municipios = municipios;
                    this.form.get('municipioId')?.enable({ emitEvent: false });

                    if (municipioId) {
                      this.form.patchValue({ municipioId: municipioId }, { emitEvent: false });

                      this.codigoPostalService.getByMunicipioId(municipioId).subscribe({
                        next: cps => {
                          this.codigosPostales = cps;
                          this.form.get('codigoPostalId')?.enable({ emitEvent: false });

                          if (codigoPostalId) {
                            this.form.patchValue({ codigoPostalId: codigoPostalId }, { emitEvent: false });
                          }
                          this.loading = false;
                        },
                        error: () => this.loading = false
                      });
                    } else {
                      this.loading = false;
                    }
                  },
                  error: () => this.loading = false
                });
              } else {
                this.loading = false;
              }
            },
            error: () => this.loading = false
          });
        } else {
          this.applyPhoneValidators(undefined);
          this.loading = false;
        }

        if (data.imagen) {
          this.logoPreviewUrl = this.systemConfigurationService.getLogoUrl(data.imagen);
        }

        if (this.isEditMode) {
          this.form.get('codigoSistema')?.disable();
        }
      },
      error: (err: any) => {
        this.error = 'Error al cargar configuración';
        console.error(err);
        this.loading = false;
      }
    });
  }

  onLogoSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;
    
    const file = input.files[0];
    const allowedExtensions = ['.png', '.jpg', '.jpeg', '.gif', '.webp'];
    const fileExtension = file.name.substring(file.name.lastIndexOf('.')).toLowerCase();
    
    if (!allowedExtensions.includes(fileExtension)) {
      this.error = 'Formato no válido. Use PNG, JPG, GIF o WEBP.';
      input.value = '';
      return;
    }
    
    if (file.size > 2 * 1024 * 1024) {
      this.error = 'El archivo no puede superar 2MB.';
      input.value = '';
      return;
    }
    
    this.error = null;
    this.selectedLogoFile = file;
    
    const reader = new FileReader();
    reader.onload = (e) => {
      this.logoPreviewUrl = e.target?.result as string;
    };
    reader.readAsDataURL(file);
  }

  removeLogo(): void {
    this.selectedLogoFile = null;
    this.logoPreviewUrl = null;
    this.form.get('imagen')?.setValue('');
  }

  onLogoError(): void {
    this.logoPreviewUrl = null;
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    
    this.saving = true; // ✅ Usar saving para guardado
    this.error = null;
    
    if (this.selectedLogoFile) {
      const codigoSistema = this.form.getRawValue().codigoSistema;
      this.systemConfigurationService.uploadLogo(codigoSistema, this.selectedLogoFile).subscribe({
        next: result => {
          this.form.get('imagen')?.setValue(result.fileName);
          this.submitForm();
        },
        error: () => {
          this.error = 'Error al subir el logo';
          this.saving = false; // ✅ Restablecer saving en error
        }
      });
    } else {
      this.submitForm();
    }
  }

  // ⭐ MÉTODO CORREGIDO - Usar saving con finalize
  private submitForm(): void {
    const dto: SystemConfigurationDTO = this.form.getRawValue();
    
    // ⭐ ASEGURAR QUE EL DTO TENGA EL ID EN MODO EDICIÓN
    if (this.isEditMode && this.configId) {
      dto.id = this.configId;
    }
    
    dto.telefono = normalizePhone(this.form.get('telefono')?.value) || '';
    dto.activo = !!this.form.get('activo')?.value;

    if (this.isEditMode) {
      // ✅ CORRECTO: Solo pasar el DTO (que ya contiene el id)
      this.systemConfigurationService.update(dto)
        .pipe(finalize(() => this.saving = false)) // ✅ Usar finalize para saving
        .subscribe({
          next: () => this.router.navigate(['/configuracion/negocio']),
          error: () => {
            this.error = 'Error al guardar configuración';
          }
        });
    } else {
      this.systemConfigurationService.create(dto)
        .pipe(finalize(() => this.saving = false)) // ✅ Usar finalize para saving
        .subscribe({
          next: () => this.router.navigate(['/configuracion/negocio']),
          error: () => {
            this.error = 'Error al guardar configuración';
          }
        });
    }
  }

  cancel(): void {
    this.router.navigate(['/configuracion/negocio']);
  }

  getErrorMessage(field: string): string {
    const control = this.form.get(field);
    if (!control || !control.errors) return '';

    if (control.errors['required']) return this.translationService.translate('validation.required');
    if (control.errors['email']) return this.translationService.translate('validation.emailInvalid');
    if (control.errors['pattern']) return 'Formato no válido';
    if (control.errors['minlength']) return `Mínimo ${control.errors['minlength'].requiredLength} caracteres`;
    if (control.errors['maxlength']) return `Máximo ${control.errors['maxlength'].requiredLength} caracteres`;
    if (control.errors['min']) return `Valor mínimo: ${control.errors['min'].min}`;
    if (control.errors['max']) return `Valor máximo: ${control.errors['max'].max}`;
    if (control.errors['telefonoInvalid']) return 'Teléfono no válido';

    return 'Error de validación';
  }
}
