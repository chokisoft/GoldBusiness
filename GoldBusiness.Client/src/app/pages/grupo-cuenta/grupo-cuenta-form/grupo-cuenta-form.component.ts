import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { GrupoCuentaService, GrupoCuentaDTO } from '../../../services/grupo-cuenta.service';
import { TranslationService } from '../../../services/translation.service';

@Component({
  selector: 'app-grupo-cuenta-form',
  templateUrl: './grupo-cuenta-form.component.html',
  styleUrls: ['./grupo-cuenta-form.component.css']
})
export class GrupoCuentaFormComponent implements OnInit {
  form: FormGroup;
  isEditMode = false;
  grupoId: number | null = null;
  loading = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private grupoCuentaService: GrupoCuentaService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslationService
  ) {
    this.form = this.fb.group({
      codigo: ['', [Validators.required, Validators.pattern(/^\d{2}$/), Validators.minLength(2), Validators.maxLength(2)]],
      descripcion: ['', [Validators.required, Validators.maxLength(256)]],
      activo: [true]
    });
  }

  ngOnInit(): void {
    this.setupFormSubscriptions();
    
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.grupoId = +id;
      this.loadGrupoCuenta();
    }
  }

  private setupFormSubscriptions(): void {
    this.form.get('descripcion')?.valueChanges.subscribe(value => {
      if (value && typeof value === 'string' && value !== value.toUpperCase()) {
        this.form.get('descripcion')?.setValue(value.toUpperCase(), { emitEvent: false });
      }
    });
  }

  loadGrupoCuenta(): void {
    if (!this.grupoId) return;

    this.loading = true;
    this.grupoCuentaService.getById(this.grupoId).subscribe({
      next: (data) => {
        this.form.patchValue(data);
        
        if (this.isEditMode) {
          this.form.get('codigo')?.disable();
        }
        
        this.loading = false;
      },
      error: (err: any) => {
        this.error = this.translate.translate('error.loading');
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

    const dto: GrupoCuentaDTO = this.form.getRawValue();

    if (this.isEditMode) {
      this.grupoCuentaService.update(this.grupoId!, dto).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/grupo-cuenta']);
        },
        error: (err: any) => {
          this.error = this.translate.translate('error.saving');
          this.loading = false;
          console.error('Error:', err);
        }
      });
    } else {
      this.grupoCuentaService.create(dto).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/grupo-cuenta']);
        },
        error: (err: any) => {
          this.error = this.translate.translate('error.saving');
          this.loading = false;
          console.error('Error:', err);
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/nomencladores/grupo-cuenta']);
  }

  getErrorMessage(fieldName: string): string {
    const control = this.form.get(fieldName);
    if (control?.hasError('required')) {
      return this.translate.translate('validation.required');
    }
    if (control?.hasError('maxlength')) {
      const maxLength = control.errors?.['maxlength'].requiredLength;
      return this.translate.translate('validation.maxLength', [maxLength]);
    }
    if (control?.hasError('pattern')) {
      return this.translate.translate('validation.codigo2Digitos');
    }
    if (control?.hasError('minlength')) {
      return this.translate.translate('validation.codigo2Digitos');
    }
    return '';
  }
}
