import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { SubGrupoCuentaService, SubGrupoCuentaDTO } from '../../../services/subgrupo-cuenta.service';
import { GrupoCuentaService, GrupoCuentaDTO } from '../../../services/grupo-cuenta.service';
import { TranslationService } from '../../../services/translation.service';

@Component({
  selector: 'app-subgrupo-cuenta-form',
  templateUrl: './subgrupo-cuenta-form.component.html',
  styleUrls: ['./subgrupo-cuenta-form.component.css']
})
export class SubGrupoCuentaFormComponent implements OnInit {
  form: FormGroup;
  isEditMode = false;
  subgrupoId: number | null = null;
  loading = false;
  loadingGrupos = true;
  error: string | null = null;
  gruposCuenta: GrupoCuentaDTO[] = [];

  constructor(
    private fb: FormBuilder,
    private subGrupoCuentaService: SubGrupoCuentaService,
    private grupoCuentaService: GrupoCuentaService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslationService
  ) {
    this.form = this.fb.group({
      codigo: ['', [Validators.required, Validators.maxLength(5)]],
      grupoCuentaId: [null, Validators.required],
      descripcion: ['', [Validators.required, Validators.maxLength(256)]],
      deudora: [true]
    });
  }

  ngOnInit(): void {
    this.loadGruposCuenta();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.subgrupoId = +id;
      this.loadSubGrupoCuenta();
    }
  }

  loadGruposCuenta(): void {
    this.loadingGrupos = true;
    this.grupoCuentaService.getAll().subscribe({
      next: (data) => {
        this.gruposCuenta = data.filter(g => g.activo);
        this.loadingGrupos = false;
      },
      error: (err) => {
        console.error('Error al cargar grupos:', err);
        this.loadingGrupos = false;
      }
    });
  }

  loadSubGrupoCuenta(): void {
    if (!this.subgrupoId) return;

    this.loading = true;
    this.subGrupoCuentaService.getById(this.subgrupoId).subscribe({
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

    const dto: SubGrupoCuentaDTO = this.form.value;

    const operation = this.isEditMode
      ? this.subGrupoCuentaService.update(this.subgrupoId!, dto)
      : this.subGrupoCuentaService.create(dto);

    operation.subscribe({
      next: () => {
        this.router.navigate(['/nomencladores/subgrupo-cuenta']);
      },
      error: (err) => {
        this.error = this.translate.translate('error.saving');
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/nomencladores/subgrupo-cuenta']);
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
