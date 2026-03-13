import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { ConceptoAjusteService, ConceptoAjusteDTO } from '../../../services/concepto-ajuste.service';
import { LanguageService } from '../../../services/language.service';
import { TranslationService } from '../../../services/translation.service';
import { CuentaService, CuentaDTO } from '../../../services/cuenta.service';

@Component({
  selector: 'app-concepto-ajuste-form',
  templateUrl: './concepto-ajuste-form.component.html',
  styleUrls: ['./concepto-ajuste-form.component.css']
})
export class ConceptoAjusteFormComponent implements OnInit, OnDestroy {
  form: FormGroup;
  isEditMode = false;
  conceptoAjusteId: number | null = null;
  loading = false;
  error: string | null = null;

  cuentas: CuentaDTO[] = [];
  loadingCuentas = true;

  private languageSubscription?: Subscription;

  constructor(
    private fb: FormBuilder,
    private conceptoAjusteService: ConceptoAjusteService,
    private cuentaService: CuentaService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslationService,
    private languageService: LanguageService
  ) {
    this.form = this.fb.group({
      id: [null],
      codigo: ['', [Validators.required, Validators.maxLength(10)]],
      descripcion: ['', [Validators.required, Validators.maxLength(256)]],
      cuentaId: ['', [Validators.required]],
      cuenta: ['', [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.setupFormSubscriptions();
    this.loadCuentas();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.conceptoAjusteId = +id;
      this.loadConceptoAjuste();
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        this.loadCuentas();
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

  loadCuentas(): void {
    this.loadingCuentas = true;
    this.cuentaService.getAll().subscribe({
      next: (data) => {
        this.cuentas = data;
        this.loadingCuentas = false;
      },
      error: (err) => {
        this.error = 'Error al cargar cuentas';
        this.loadingCuentas = false;
      }
    });
  }

  loadConceptoAjuste(): void {
    if (!this.conceptoAjusteId) return;

    this.loading = true;
    this.conceptoAjusteService.getById(this.conceptoAjusteId).subscribe({
      next: (data) => {
        this.form.patchValue(data);
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar el conceptoAjuste';
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

    const formData: ConceptoAjusteDTO = this.form.getRawValue();

    if (this.isEditMode) {
      this.conceptoAjusteService.update(this.conceptoAjusteId!, formData).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/concepto-ajuste']);
        },
        error: (err) => {
          this.loading = false;
          this.error = 'Error al actualizar';
        }
      });
    } else {
      this.conceptoAjusteService.create(formData).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/concepto-ajuste']);
        },
        error: (err) => {
          this.loading = false;
          this.error = 'Error al crear';
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/nomencladores/concepto-ajuste']);
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
