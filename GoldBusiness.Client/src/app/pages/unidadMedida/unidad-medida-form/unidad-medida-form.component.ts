import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { UnidadMedidaService, UnidadMedidaDTO } from '../../../services/unidad-medida.service';
import { LanguageService } from '../../../services/language.service';
import { TranslationService } from '../../../services/translation.service';

@Component({
  selector: 'app-unidad-medida-form',
  templateUrl: './unidad-medida-form.component.html',
  styleUrls: ['./unidad-medida-form.component.css']
})
export class UnidadMedidaFormComponent implements OnInit, OnDestroy {
  form: FormGroup;
  isEditMode = false;
  unidadMedidaId: number | null = null;
  loading = false;
  error: string | null = null;

  private languageSubscription?: Subscription;

  constructor(
    private fb: FormBuilder,
    private unidadMedidaService: UnidadMedidaService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslationService,
    private languageService: LanguageService
  ) {
    this.form = this.fb.group({
      codigo: ['', [
        Validators.required, 
        Validators.minLength(3), 
        Validators.maxLength(3),
        Validators.pattern(/^[A-Z0-9]{3}$/) // ✅ Validación alfanumérica mayúsculas de 3 caracteres
      ]],
      descripcion: ['', [
        Validators.required, 
        Validators.maxLength(256)
      ]]
    });
  }

  ngOnInit(): void {
    this.setupFormSubscriptions();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.unidadMedidaId = +id;
      this.loadUnidadMedida();
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        // Recargar si es necesario
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  private setupFormSubscriptions(): void {
    // Auto uppercase codigo
    this.form.get('codigo')?.valueChanges.subscribe(value => {
      if (value && typeof value === 'string' && value !== value.toUpperCase()) {
        this.form.get('codigo')?.setValue(value.toUpperCase(), { emitEvent: false });
      }
    });

    // Auto uppercase descripcion
    this.form.get('descripcion')?.valueChanges.subscribe(value => {
      if (value && typeof value === 'string' && value !== value.toUpperCase()) {
        this.form.get('descripcion')?.setValue(value.toUpperCase(), { emitEvent: false });
      }
    });
  }

  loadUnidadMedida(): void {
    if (!this.unidadMedidaId) return;

    this.loading = true;
    this.unidadMedidaService.getById(this.unidadMedidaId).subscribe({
      next: (data) => {
        this.form.patchValue(data);
        
        // ✅ Deshabilitar código en modo edición
        if (this.isEditMode) {
          this.form.get('codigo')?.disable();
        }
        
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar la unidad de medida';
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      Object.keys(this.form.controls).forEach(key => {
        this.form.get(key)?.markAsTouched();
      });
      return;
    }

    this.loading = true;
    this.error = null;

    const formData: UnidadMedidaDTO = this.form.getRawValue(); // ✅ getRawValue() para incluir campos deshabilitados

    if (this.isEditMode) {
      this.unidadMedidaService.update(this.unidadMedidaId!, formData).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/unidad-medida']);
        },
        error: (err) => {
          this.loading = false;
          this.error = 'Error al actualizar';
        }
      });
    } else {
      this.unidadMedidaService.create(formData).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/unidad-medida']);
        },
        error: (err) => {
          this.loading = false;
          this.error = 'Error al crear';
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/nomencladores/unidad-medida']);
  }

  getErrorMessage(fieldName: string): string {
    const control = this.form.get(fieldName);
    if (!control || !control.errors) return '';

    const errors = control.errors;
    if (errors['required']) return this.translate.translate('validation.required');
    if (errors['pattern']) return this.translate.translate('validation.alphanumeric');
    if (errors['minlength'] || errors['maxlength']) return this.translate.translate('validation.length');

    return this.translate.translate('validation.invalid');
  }
}
