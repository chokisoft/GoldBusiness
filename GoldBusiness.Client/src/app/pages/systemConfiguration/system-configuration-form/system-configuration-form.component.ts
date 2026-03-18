import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { CuentaService, CuentaDTO } from '../../../services/cuenta.service';
import { LanguageService } from '../../../services/language.service';
import { SystemConfigurationService, SystemConfigurationDTO, Pais } from '../../../services/system-configuration.service';
import { PaisService, PaisDTO } from '../../../services/pais.service';
import { ProvinciaService, ProvinciaDTO } from '../../../services/provincia.service';
import { MunicipioService, MunicipioDTO } from '../../../services/municipio.service';
import { CodigoPostalService, CodigoPostalDTO } from '../../../services/codigo-postal.service';

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

  // Keep Pais type from systemConfiguration service; use DTO types for provinces/municipios/cps
  paises: Pais[] = [];
  provincias: ProvinciaDTO[] = [];
  municipios: MunicipioDTO[] = [];
  codigosPostales: CodigoPostalDTO[] = [];

  // Flags de carga para mostrar estado en UI
  loadingProvincias = false;
  loadingMunicipios = false;
  loadingCodigosPostales = false;

  // Phone helper: pais detallado y sub de detalle
  selectedPais?: PaisDTO;
  private paisDetailSub?: Subscription;

  private languageSubscription?: Subscription;
  private paisSub?: Subscription;
  private provinciaSub?: Subscription;
  private municipioSub?: Subscription;

  constructor(
    private fb: FormBuilder,
    private systemConfigurationService: SystemConfigurationService,
    private cuentaService: CuentaService,
    private router: Router,
    private route: ActivatedRoute,
    private languageService: LanguageService,
    private paisService: PaisService,
    private provinciaService: ProvinciaService,
    private municipioService: MunicipioService,
    private codigoPostalService: CodigoPostalService
  ) {
    this.form = this.fb.group({
      codigoSistema: ['', [Validators.required, Validators.maxLength(3)]],
      licencia: ['', [Validators.required, Validators.maxLength(100)]],
      nombreNegocio: ['', [Validators.required, Validators.maxLength(256)]],
      direccion: ['', Validators.maxLength(512)],
      paisId: [null, Validators.required],
      provinciaId: [{ value: null, disabled: true }, Validators.required],
      municipioId: [{ value: null, disabled: true }, Validators.required],
      codigoPostalId: [{ value: null, disabled: true }, Validators.required],
      imagen: ['', Validators.maxLength(500)],
      web: ['', Validators.maxLength(256)],
      email: ['', [Validators.email, Validators.maxLength(256)]],
      telefono: ['', Validators.maxLength(50)],
      cuentaPagarId: [{ value: null, disabled: this.loadingCuentas }, Validators.required],
      cuentaCobrarId: [{ value: null, disabled: this.loadingCuentas }, Validators.required],
      caducidad: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.setupFormSubscriptions();
    this.loadCuentas();
    this.loadPaises();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.configId = +id;
      this.loadConfiguration();
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        this.loadCuentas();
        this.loadPaises();
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

  loadPaises(): void {
    this.systemConfigurationService.getPaises().subscribe({
      next: data => this.paises = data,
      error: err => {
        console.error('Error al cargar países', err);
        this.error = 'Error al cargar países';
      }
    });
  }

  loadProvincias(paisId: number): void {
    this.loadingProvincias = true;
    this.provinciaService.getByPaisId(paisId).subscribe({
      next: (data) => {
        // provinciaService returns ProvinciaDTO[]
        this.provincias = data;
        this.loadingProvincias = false;
      },
      error: err => {
        console.error('Error al cargar provincias', err);
        this.loadingProvincias = false;
        this.error = 'Error al cargar provincias';
      }
    });
  }

  loadMunicipios(provinciaId: number): void {
    this.loadingMunicipios = true;
    this.municipioService.getByProvinciaId(provinciaId).subscribe({
      next: (data) => {
        // municipioService returns MunicipioDTO[]
        this.municipios = data;
        this.loadingMunicipios = false;
      },
      error: err => {
        console.error('Error al cargar municipios', err);
        this.loadingMunicipios = false;
        this.error = 'Error al cargar municipios';
      }
    });
  }

  loadCodigosPostales(municipioId: number): void {
    this.loadingCodigosPostales = true;
    this.codigoPostalService.getByMunicipioId(municipioId).subscribe({
      next: (data) => {
        // codigoPostalService returns CodigoPostalDTO[]
        this.codigosPostales = data;
        this.loadingCodigosPostales = false;
      },
      error: err => {
        console.error('Error al cargar códigos postales', err);
        this.loadingCodigosPostales = false;
        this.error = 'Error al cargar códigos postales';
      }
    });
  }

  private applyPhoneValidators(pais?: PaisDTO): void {
    this.selectedPais = pais;
    const telefono = this.form.get('telefono')!;

    const baseValidators = [Validators.maxLength(50)];
    if (pais && pais.regexTelefono) {
      try {
        const re = new RegExp(pais.regexTelefono);
        const phonePatternValidator: ValidatorFn = (c: AbstractControl) => {
          if (!c.value) return null;
          return re.test(c.value) ? null : { telefonoInvalid: true };
        };
        telefono.setValidators([...baseValidators, phonePatternValidator]);
      } catch {
        telefono.setValidators(baseValidators);
        console.warn('Invalid regexTelefono from API for pais', pais?.id);
      }
    } else {
      telefono.setValidators(baseValidators);
    }

    telefono.updateValueAndValidity({ emitEvent: false });
  }

  extractId(eventOrId: Event | number): number {
    if (typeof eventOrId === 'number') return eventOrId;
    const target = (eventOrId as Event).target as HTMLSelectElement | null;
    const val = target?.value ?? (eventOrId as any);
    return val ? Number(val) : 0;
  }

  loadCuentas(): void {
    this.loadingCuentas = true;
    this.cuentaService.getAll().subscribe({
      next: (data) => {
        this.cuentas = data.filter(c => !c.cancelado);
        this.loadingCuentas = false;
        this.form.get('cuentaPagarId')?.enable({ emitEvent: false });
        this.form.get('cuentaCobrarId')?.enable({ emitEvent: false });
      },
      error: () => {
        this.loadingCuentas = false;
        this.form.get('cuentaPagarId')?.enable({ emitEvent: false });
        this.form.get('cuentaCobrarId')?.enable({ emitEvent: false });
      }
    });
  }

  loadConfiguration(): void {
    if (!this.configId) return;
    this.loading = true;
    this.systemConfigurationService.getById(this.configId).subscribe({
      next: data => {
        const formattedCaducidad = data.caducidad
          ? new Date(data.caducidad).toISOString().substring(0, 10)
          : '';

        this.form.patchValue({
          codigoSistema: data.codigoSistema,
          licencia: data.licencia,
          nombreNegocio: data.nombreNegocio,
          direccion: data.direccion,
          imagen: data.imagen,
          web: data.web,
          email: data.email,
          telefono: data.telefono,
          cuentaPagarId: data.cuentaPagarId,
          cuentaCobrarId: data.cuentaCobrarId,
          caducidad: formattedCaducidad
        }, { emitEvent: false });

        const paisId = data.paisId ?? 0;
        const provinciaId = data.provinciaId ?? 0;
        const municipioId = data.municipioId ?? 0;
        const codigoPostalId = data.codigoPostalId ?? 0;

        if (!paisId) {
          this.applyPhoneValidators(undefined);
          this.loading = false;
          return;
        }

        // set paisId first to keep select state consistent
        this.form.patchValue({ paisId: paisId }, { emitEvent: false });

        this.paisDetailSub?.unsubscribe();
        this.paisDetailSub = this.paisService.getById(paisId).subscribe({
          next: p => this.applyPhoneValidators(p),
          error: () => this.applyPhoneValidators(undefined)
        });

        // use provinciaService/municipioService/codigoPostalService which return DTO[] types
        this.provinciaService.getByPaisId(paisId).subscribe({
          next: provinces => {
            this.provincias = provinces;
            this.form.get('provinciaId')?.enable({ emitEvent: false });

            if (!provinciaId) {
              this.loading = false;
              return;
            }

            this.form.patchValue({ provinciaId: provinciaId }, { emitEvent: false });

            this.municipioService.getByProvinciaId(provinciaId).subscribe({
              next: municipios => {
                this.municipios = municipios;
                this.form.get('municipioId')?.enable({ emitEvent: false });

                if (!municipioId) {
                  this.loading = false;
                  return;
                }

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
                  error: () => {
                    this.loading = false;
                  }
                });
              },
              error: () => {
                this.loading = false;
              }
            });
          },
          error: () => {
            this.loading = false;
          }
        });

        if (data.imagen) {
          this.logoPreviewUrl = this.systemConfigurationService.getLogoUrl(data.imagen);
        }

        if (this.isEditMode) this.form.get('codigoSistema')?.disable();
      },
      error: err => {
        this.error = 'Error al cargar configuración';
        this.loading = false;
        console.error('Error cargando configuración:', err);
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
    reader.onload = (e) => { this.logoPreviewUrl = e.target?.result as string; };
    reader.readAsDataURL(file);
  }

  removeLogo(): void {
    this.selectedLogoFile = null;
    this.logoPreviewUrl = null;
    this.form.get('imagen')?.setValue('');
  }

  onLogoError(): void { this.logoPreviewUrl = null; }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.loading = true;
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
          this.loading = false;
        }
      });
    } else {
      this.submitForm();
    }
  }

  private submitForm(): void {
    const dto: SystemConfigurationDTO = this.form.getRawValue();
    if (this.isEditMode) {
      this.systemConfigurationService.update(this.configId!, dto).subscribe({
        next: () => this.router.navigate(['/configuracion/negocio']),
        error: () => {
          this.error = 'Error al guardar configuración';
          this.loading = false;
        }
      });
    } else {
      this.systemConfigurationService.create(dto).subscribe({
        next: () => this.router.navigate(['/configuracion/negocio']),
        error: () => {
          this.error = 'Error al guardar configuración';
          this.loading = false;
        }
      });
    }
  }

  cancel(): void { this.router.navigate(['/configuracion/negocio']); }

  getErrorMessage(fieldName: string): string {
    const control = this.form.get(fieldName);
    if (control?.hasError('required')) return 'Este campo es requerido';
    if (control?.hasError('maxlength')) {
      return `Máximo ${control.errors?.['maxlength'].requiredLength} caracteres`;
    }
    if (control?.hasError('email')) return 'Email no válido';
    if (control?.hasError('telefonoInvalid')) return 'Teléfono no válido';
    return '';
  }
}
