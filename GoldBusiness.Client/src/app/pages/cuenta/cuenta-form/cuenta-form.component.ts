import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CuentaService, CuentaDTO } from '../../../services/cuenta.service';
import { SubGrupoCuentaService, SubGrupoCuentaDTO } from '../../../services/subgrupo-cuenta.service';
import { TranslationService } from '../../../services/translation.service';

@Component({
  selector: 'app-cuenta-form',
  templateUrl: './cuenta-form.component.html',
  styleUrls: ['./cuenta-form.component.css']
})
export class CuentaFormComponent implements OnInit {
  form: FormGroup;
  isEditMode = false;
  cuentaId: number | null = null;
  loading = false;
  loadingSubgrupos = true;
  error: string | null = null;
  subgruposCuenta: SubGrupoCuentaDTO[] = [];

  constructor(
    private fb: FormBuilder,
    private cuentaService: CuentaService,
    private subGrupoCuentaService: SubGrupoCuentaService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslationService
  ) {
    this.form = this.fb.group({
      codigo: ['', [Validators.required, Validators.maxLength(10)]],
      subGrupoCuentaId: [null, Validators.required],
      descripcion: ['', [Validators.required, Validators.maxLength(256)]]
    });
  }

  ngOnInit(): void {
    this.loadSubgruposCuenta();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.cuentaId = +id;
      this.loadCuenta();
    }
  }

  loadSubgruposCuenta(): void {
    this.loadingSubgrupos = true;
    this.subGrupoCuentaService.getAll().subscribe({
      next: (data) => {
        this.subgruposCuenta = data.filter(s => !s.cancelado);
        this.loadingSubgrupos = false;
      },
      error: (err) => {
        console.error('Error al cargar subgrupos:', err);
        this.loadingSubgrupos = false;
      }
    });
  }

  loadCuenta(): void {
    if (!this.cuentaId) return;

    this.loading = true;
    this.cuentaService.getById(this.cuentaId).subscribe({
      next: (data) => {
        this.form.patchValue(data);
        this.loading = false;
      },
      error: (err) => {
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

    const dto: CuentaDTO = this.form.value;

    const operation = this.isEditMode
      ? this.cuentaService.update(this.cuentaId!, dto)
      : this.cuentaService.create(dto);

    operation.subscribe({
      next: () => {
        this.router.navigate(['/nomencladores/cuenta']);
      },
      error: (err) => {
        this.error = this.translate.translate('error.saving');
        this.loading = false;
        console.error('Error:', err);
      }
    });
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
