import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { TransaccionService, TransaccionDTO } from '../../../services/transaccion.service';
import { LanguageService } from '../../../services/language.service';
import { TranslationService } from '../../../services/translation.service';

@Component({
  selector: 'app-transaccion-form',
  templateUrl: './transaccion-form.component.html',
  styleUrls: ['./transaccion-form.component.css']
})
export class TransaccionFormComponent implements OnInit, OnDestroy {
  form: FormGroup;
  isEditMode = false;
  transaccionId: number | null = null;
  loading = false;
  error: string | null = null;


  private languageSubscription?: Subscription;

  constructor(
    private fb: FormBuilder,
    private transaccionService: TransaccionService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslationService,
    private languageService: LanguageService
  ) {
    this.form = this.fb.group({
      id: ['', [Validators.required]],
      codigo: [''],
      descripcion: [''],
    });
  }

  ngOnInit(): void {
    this.setupFormSubscriptions();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.transaccionId = +id;
      this.loadTransaccion();
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  private setupFormSubscriptions(): void {
    const upperFields = ['descripcion'];
    upperFields.forEach(field => {
      this.form.get(field)?.valueChanges.subscribe(value => {
        if (value && typeof value === 'string' && value !== value.toUpperCase()) {
          this.form.get(field)?.setValue(value.toUpperCase(), { emitEvent: false });
        }
      });
    });
  }

  loadTransaccion(): void {
    if (!this.transaccionId) return;

    this.loading = true;
    this.transaccionService.getById(this.transaccionId).subscribe({
      next: (data) => {
        this.form.patchValue(data);
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar el transaccion';
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

    const formData: TransaccionDTO = this.form.getRawValue();

    if (this.isEditMode) {
      this.transaccionService.update(this.transaccionId!, formData).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/transaccion']);
        },
        error: (err) => {
          this.loading = false;
          this.error = 'Error al actualizar';
        }
      });
    } else {
      this.transaccionService.create(formData).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/transaccion']);
        },
        error: (err) => {
          this.loading = false;
          this.error = 'Error al crear';
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/nomencladores/transaccion']);
  }

  getErrorMessage(fieldName: string): string {
    const control = this.form.get(fieldName);
    if (!control || !control.errors) return '';

    if (control.errors['required']) return 'Campo requerido';
    if (control.errors['maxlength']) {
      const max = control.errors['maxlength'].requiredLength;
      return `Máximo ${max} caracteres`;
    }
    if (control.errors['pattern']) return 'Formato inválido';
    return 'Campo inválido';
  }
}
