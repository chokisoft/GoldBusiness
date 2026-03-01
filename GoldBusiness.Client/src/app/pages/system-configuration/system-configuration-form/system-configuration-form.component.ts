import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { CuentaService, CuentaDTO } from '../../../services/cuenta.service';
import { LanguageService } from '../../../services/language.service';
import { SystemConfigurationService, SystemConfigurationDTO, Pais, Provincia, Municipio, CodigoPostal } from '../../../services/system-configuration.service';

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
  error: string | null = null;
  cuentas: CuentaDTO[] = [];
  loadingCuentas = true;

  selectedLogoFile: File | null = null;
  logoPreviewUrl: string | null = null;

  paises: Pais[] = [];
  provincias: Provincia[] = [];
  municipios: Municipio[] = [];
  codigosPostales: CodigoPostal[] = [];

  // Flags de carga para mostrar estado en UI
  loadingProvincias = false;
  loadingMunicipios = false;
  loadingCodigosPostales = false;

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
    private languageService: LanguageService
  ) {
    this.form = this.fb.group({
      codigoSistema: ['', [Validators.required, Validators.maxLength(50)]],
      licencia: ['', [Validators.required, Validators.maxLength(100)]],
      nombreNegocio: ['', [Validators.required, Validators.maxLength(256)]],
      direccion: ['', Validators.maxLength(512)],
      // Controls dependientes se crean deshabilitados por defecto
      paisId: [null, Validators.required],
      provinciaId: [{ value: null, disabled: true }, Validators.required],
      municipioId: [{ value: null, disabled: true }, Validators.required],
      codigoPostalId: [{ value: null, disabled: true }, Validators.required],
      imagen: ['', Validators.maxLength(500)],
      web: ['', Validators.maxLength(256)],
      email: ['', [Validators.email, Validators.maxLength(256)]],
      telefono: ['', Validators.maxLength(20)],
      // Cuentas deshabilitadas mientras cargan
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
  }

  private setupFormSubscriptions(): void {
    const upperFields = ['nombreNegocio', 'direccion'];
    upperFields.forEach(field => {
      this.form.get(field)?.valueChanges.subscribe(value => {
        if (value && typeof value === 'string' && value !== value.toUpperCase()) {
          this.form.get(field)?.setValue(value.toUpperCase(), { emitEvent: false });
        }
      });
    });

    // Cuando cambia el país habilitamos/deshabilitamos y cargamos provincias
    this.paisSub = this.form.get('paisId')?.valueChanges.subscribe((paisId: number) => {
      console.debug('paisId changed ->', paisId);
      const provControl = this.form.get('provinciaId');
      // limpiar dependientes
      this.provincias = [];
      this.municipios = [];
      this.codigosPostales = [];
      this.form.patchValue({ provinciaId: null, municipioId: null, codigoPostalId: null }, { emitEvent: false });

      if (paisId) {
        provControl?.enable({ emitEvent: false });
        this.loadProvincias(paisId);
      } else {
        provControl?.disable({ emitEvent: false });
      }
    });

    // Cuando cambia la provincia habilitamos/deshabilitamos y cargamos municipios
    this.provinciaSub = this.form.get('provinciaId')?.valueChanges.subscribe((provinciaId: number) => {
      console.debug('provinciaId changed ->', provinciaId);
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

    // Cuando cambia el municipio habilitamos/deshabilitamos y cargamos códigos postales
    this.municipioSub = this.form.get('municipioId')?.valueChanges.subscribe((municipioId: number) => {
      console.debug('municipioId changed ->', municipioId);
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
      next: (data) => this.paises = data,
      error: () => this.error = 'Error al cargar países'
    });
  }

  loadProvincias(paisId: number): void {
    this.loadingProvincias = true;
    this.systemConfigurationService.getProvinciasByPais(paisId).subscribe({
      next: (data) => {
        this.provincias = data;
        this.loadingProvincias = false;
      },
      error: () => {
        this.loadingProvincias = false;
        this.error = 'Error al cargar provincias';
      }
    });
  }

  loadMunicipios(provinciaId: number): void {
    this.loadingMunicipios = true;
    this.systemConfigurationService.getMunicipiosByProvincia(provinciaId).subscribe({
      next: (data) => {
        this.municipios = data;
        this.loadingMunicipios = false;
      },
      error: () => {
        this.loadingMunicipios = false;
        this.error = 'Error al cargar municipios';
      }
    });
  }

  loadCodigosPostales(municipioId: number): void {
    this.loadingCodigosPostales = true;
    this.systemConfigurationService.getCodigosPostalesByMunicipio(municipioId).subscribe({
      next: (data) => {
        this.codigosPostales = data;
        this.loadingCodigosPostales = false;
      },
      error: () => {
        this.loadingCodigosPostales = false;
        this.error = 'Error al cargar códigos postales';
      }
    });
  }

  private extractId(eventOrId: Event | number): number {
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
      next: (data) => {
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

        const paisId = data.paisId;
        const provinciaId = data.provinciaId;
        const municipioId = data.municipioId;
        const codigoPostalId = data.codigoPostalId;

        // Cargar dependientes secuencialmente y establecer valores sin disparar eventos
        if (paisId) {
          this.systemConfigurationService.getProvinciasByPais(paisId).subscribe({
            next: (provs) => {
              this.provincias = provs;
              this.form.get('provinciaId')?.enable({ emitEvent: false });
              this.form.patchValue({ paisId: paisId }, { emitEvent: false });

              if (provinciaId) {
                this.systemConfigurationService.getMunicipiosByProvincia(provinciaId).subscribe({
                  next: (muns) => {
                    this.municipios = muns;
                    this.form.get('municipioId')?.enable({ emitEvent: false });
                    this.form.patchValue({ provinciaId: provinciaId }, { emitEvent: false });

                    if (municipioId) {
                      this.systemConfigurationService.getCodigosPostalesByMunicipio(municipioId).subscribe({
                        next: (cods) => {
                          this.codigosPostales = cods;
                          this.form.get('codigoPostalId')?.enable({ emitEvent: false });
                          this.form.patchValue({ municipioId: municipioId, codigoPostalId: codigoPostalId }, { emitEvent: false });
                        },
                        error: () => console.warn('Error cargando códigos postales')
                      });
                    }
                  },
                  error: () => console.warn('Error cargando municipios')
                });
              }
            },
            error: () => console.warn('Error cargando provincias')
          });
        }

        if (data.imagen) {
          this.logoPreviewUrl = this.systemConfigurationService.getLogoUrl(data.imagen);
        }

        if (this.isEditMode) this.form.get('codigoSistema')?.disable();

        this.loading = false;
      },
      error: (err: any) => {
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
        next: (result) => {
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
    return '';
  }
}
