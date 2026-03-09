import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { LineaService, LineaDTO } from '../../../services/linea.service';
import { LanguageService } from '../../../services/language.service';
import { TranslationService } from '../../../services/translation.service';

@Component({
  selector: 'app-linea-form',
  templateUrl: './linea-form.component.html',
  styleUrls: ['./linea-form.component.css']
})
export class LineaFormComponent implements OnInit, OnDestroy {
  form: FormGroup;
  isEditMode = false;
  lineaId: number | null = null;
  loading = false;
  error: string | null = null;

  private languageSubscription?: Subscription;

  constructor(
    private fb: FormBuilder,
    private lineaService: LineaService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslationService,
    private languageService: LanguageService
  ) {
    this.form = this.fb.group({
      codigo: ['', [
        Validators.required, 
        Validators.minLength(2), 
        Validators.maxLength(2),
        Validators.pattern(/^\d{2}$/) // ✅ Validación numérica de 2 dígitos
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
      this.lineaId = +id;
      this.loadLinea();
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
    // Auto uppercase descripcion
    this.form.get('descripcion')?.valueChanges.subscribe(value => {
      if (value && typeof value === 'string' && value !== value.toUpperCase()) {
        this.form.get('descripcion')?.setValue(value.toUpperCase(), { emitEvent: false });
      }
    });
  }

  loadLinea(): void {
    if (!this.lineaId) return;

    this.loading = true;
    this.lineaService.getById(this.lineaId).subscribe({
      next: (data) => {
        this.form.patchValue(data);
        
        // ✅ Deshabilitar código en modo edición
        if (this.isEditMode) {
          this.form.get('codigo')?.disable();
        }
        
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar la línea';
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

    const formData: LineaDTO = this.form.getRawValue(); // ✅ getRawValue() para incluir campos deshabilitados

    if (this.isEditMode) {
      this.lineaService.update(this.lineaId!, formData).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/linea']);
        },
        error: (err) => {
          this.loading = false;
          this.error = 'Error al actualizar';
        }
      });
    } else {
      this.lineaService.create(formData).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/linea']);
        },
        error: (err) => {
          this.loading = false;
          this.error = 'Error al crear';
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/nomencladores/linea']);
  }

  getErrorMessage(fieldName: string): string {
    const control = this.form.get(fieldName);
    if (!control || !control.errors) return '';

    const errors = control.errors;
    if (errors['required']) return this.translate.translate('validation.required');
    if (errors['pattern']) return this.translate.translate('validation.numeric');
    if (errors['minlength'] || errors['maxlength']) return this.translate.translate('validation.length');

    return this.translate.translate('validation.invalid');
  }
}
