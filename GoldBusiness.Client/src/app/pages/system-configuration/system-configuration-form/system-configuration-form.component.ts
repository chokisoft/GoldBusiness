import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { SystemConfigurationService, SystemConfigurationDTO } from '../../../services/system-configuration.service';
import { CuentaService, CuentaDTO } from '../../../services/cuenta.service';

@Component({
  selector: 'app-system-configuration-form',
  templateUrl: './system-configuration-form.component.html',
  styleUrls: ['./system-configuration-form.component.css']
})
export class SystemConfigurationFormComponent implements OnInit {
  form: FormGroup;
  isEditMode = false;
  configId: number | null = null;
  loading = false;
  error: string | null = null;
  cuentas: CuentaDTO[] = [];
  loadingCuentas = true;

  constructor(
    private fb: FormBuilder,
    private systemConfigurationService: SystemConfigurationService,
    private cuentaService: CuentaService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.form = this.fb.group({
      codigoSistema: ['', [Validators.required, Validators.maxLength(50)]],
      licencia: ['', [Validators.required, Validators.maxLength(100)]],
      nombreNegocio: ['', [Validators.required, Validators.maxLength(256)]],
      direccion: ['', Validators.maxLength(512)],
      municipio: ['', Validators.maxLength(128)],
      provincia: ['', Validators.maxLength(128)],
      codPostal: ['', Validators.maxLength(20)],
      imagen: ['', Validators.maxLength(500)],
      web: ['', Validators.maxLength(256)],
      email: ['', [Validators.email, Validators.maxLength(256)]],
      telefono: ['', Validators.maxLength(20)],
      cuentaPagarId: [null, Validators.required],
      cuentaCobrarId: [null, Validators.required],
      caducidad: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.setupFormSubscriptions();
    this.loadCuentas();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.configId = +id;
      this.loadConfiguration();
    }
  }

  private setupFormSubscriptions(): void {
    this.form.get('nombreNegocio')?.valueChanges.subscribe(value => {
      if (value && typeof value === 'string' && value !== value.toUpperCase()) {
        this.form.get('nombreNegocio')?.setValue(value.toUpperCase(), { emitEvent: false });
      }
    });

    this.form.get('direccion')?.valueChanges.subscribe(value => {
      if (value && typeof value === 'string' && value !== value.toUpperCase()) {
        this.form.get('direccion')?.setValue(value.toUpperCase(), { emitEvent: false });
      }
    });

    this.form.get('municipio')?.valueChanges.subscribe(value => {
      if (value && typeof value === 'string' && value !== value.toUpperCase()) {
        this.form.get('municipio')?.setValue(value.toUpperCase(), { emitEvent: false });
      }
    });

    this.form.get('provincia')?.valueChanges.subscribe(value => {
      if (value && typeof value === 'string' && value !== value.toUpperCase()) {
        this.form.get('provincia')?.setValue(value.toUpperCase(), { emitEvent: false });
      }
    });
  }

  loadCuentas(): void {
    this.loadingCuentas = true;
    this.cuentaService.getAll().subscribe({
      next: (data) => {
        this.cuentas = data.filter(c => !c.cancelado);
        this.loadingCuentas = false;
      },
      error: (err: any) => {
        console.error('Error al cargar cuentas:', err);
        this.loadingCuentas = false;
      }
    });
  }

  loadConfiguration(): void {
    if (!this.configId) return;

    this.loading = true;
    this.systemConfigurationService.getById(this.configId).subscribe({
      next: (data) => {
        this.form.patchValue(data);

        if (this.isEditMode) {
          this.form.get('codigoSistema')?.disable();
        }

        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Error al cargar configuración';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading = true;
    this.error = null;

    const dto: SystemConfigurationDTO = this.form.getRawValue();

    if (this.isEditMode) {
      this.systemConfigurationService.update(this.configId!, dto).subscribe({
        next: () => {
          this.router.navigate(['/configuracion']);
        },
        error: (err: any) => {
          this.error = 'Error al guardar configuración';
          this.loading = false;
          console.error('Error:', err);
        }
      });
    } else {
      this.systemConfigurationService.create(dto).subscribe({
        next: () => {
          this.router.navigate(['/configuracion']);
        },
        error: (err: any) => {
          this.error = 'Error al guardar configuración';
          this.loading = false;
          console.error('Error:', err);
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/configuracion']);
  }

  getErrorMessage(fieldName: string): string {
    const control = this.form.get(fieldName);
    if (control?.hasError('required')) {
      return 'Este campo es requerido';
    }
    if (control?.hasError('maxlength')) {
      const maxLength = control.errors?.['maxlength'].requiredLength;
      return `Máximo ${maxLength} caracteres`;
    }
    if (control?.hasError('email')) {
      return 'Email no válido';
    }
    return '';
  }
}
