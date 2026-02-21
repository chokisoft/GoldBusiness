import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { CuentaService, CuentaDTO } from '../../../services/cuenta.service';
import { SubGrupoCuentaService, SubGrupoCuentaDTO } from '../../../services/subgrupo-cuenta.service';
import { TranslationService } from '../../../services/translation.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-cuenta-form',
  templateUrl: './cuenta-form.component.html',
  styleUrls: ['./cuenta-form.component.css']
})
export class CuentaFormComponent implements OnInit, OnDestroy {
  form: FormGroup;
  isEditMode = false;
  cuentaId: number | null = null;
  loading = false;
  error: string | null = null;
  subGruposCuenta: SubGrupoCuentaDTO[] = [];
  loadingSubGrupos = true;

  private languageSubscription?: Subscription;

  constructor(
    private fb: FormBuilder,
    private cuentaService: CuentaService,
    private subGrupoCuentaService: SubGrupoCuentaService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslationService,
    private languageService: LanguageService
  ) {
    this.form = this.fb.group({
      codigo: ['', [Validators.required, Validators.maxLength(10)]],
      subGrupoCuentaId: [null, Validators.required],
      descripcion: ['', [Validators.required, Validators.maxLength(256)]],
      systemConfigurationId: [1]
    });
  }

  ngOnInit(): void {
    this.setupFormSubscriptions();
    this.loadSubGruposCuenta();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.cuentaId = +id;
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 CuentaForm: Idioma cambiado, recargando datos...');
        this.loadSubGruposCuenta(); // chains to loadCuenta() in edit mode
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  private setupFormSubscriptions(): void {
    this.form.get('descripcion')?.valueChanges.subscribe(value => {
      if (value && typeof value === 'string' && value !== value.toUpperCase()) {
        this.form.get('descripcion')?.setValue(value.toUpperCase(), { emitEvent: false });
      }
    });
  }

  loadSubGruposCuenta(): void {
    this.loadingSubGrupos = true;
    this.subGrupoCuentaService.getAll().subscribe({
      next: (data) => {
        this.subGruposCuenta = data.filter(s => !s.cancelado);
        this.loadingSubGrupos = false;

        if (this.isEditMode && this.cuentaId) {
          this.loadCuenta();
        }
      },
      error: (err: any) => {
        console.error('Error al cargar subgrupos:', err);
        this.error = 'Error al cargar subgrupos de cuenta';
        this.loadingSubGrupos = false;
      }
    });
  }

  loadCuenta(): void {
    if (!this.cuentaId) return;

    this.loading = true;
    this.cuentaService.getById(this.cuentaId).subscribe({
      next: (data) => {
        this.form.patchValue(data);

        if (this.isEditMode) {
          this.form.get('codigo')?.disable();
          this.form.get('subGrupoCuentaId')?.disable();
        }

        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Error al cargar la cuenta';
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

    const dto: CuentaDTO = this.form.getRawValue();

    if (this.isEditMode) {
      this.cuentaService.update(this.cuentaId!, dto).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/cuenta']);
        },
        error: (err: any) => {
          this.error = 'Error al guardar la cuenta';
          this.loading = false;
          console.error('Error:', err);
        }
      });
    } else {
      this.cuentaService.create(dto).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/cuenta']);
        },
        error: (err: any) => {
          this.error = 'Error al guardar la cuenta';
          this.loading = false;
          console.error('Error:', err);
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/nomencladores/cuenta']);
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
    return '';
  }
}
