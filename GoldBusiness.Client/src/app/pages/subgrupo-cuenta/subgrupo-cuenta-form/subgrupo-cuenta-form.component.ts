import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { SubGrupoCuentaService, SubGrupoCuentaDTO } from '../../../services/subgrupo-cuenta.service';
import { GrupoCuentaService, GrupoCuentaDTO } from '../../../services/grupo-cuenta.service';

@Component({
  selector: 'app-subgrupo-cuenta-form',
  templateUrl: './subgrupo-cuenta-form.component.html',
  styleUrls: ['./subgrupo-cuenta-form.component.css']
})
export class SubGrupoCuentaFormComponent implements OnInit {
  form: FormGroup;
  gruposCuenta: GrupoCuentaDTO[] = [];
  isEditMode = false;
  subgrupoId: number | null = null;
  loading = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private subGrupoCuentaService: SubGrupoCuentaService,
    private grupoCuentaService: GrupoCuentaService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.form = this.fb.group({
      codigo: ['', [Validators.required, Validators.pattern(/^\d{5}$/)]],
      grupoCuentaId: [null, [Validators.required, Validators.min(1)]],
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
    this.grupoCuentaService.getAll().subscribe({
      next: (data) => {
        this.gruposCuenta = data.filter(g => g.activo);
      },
      error: (err) => {
        console.error('Error al cargar grupos de cuenta:', err);
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
        this.error = 'Error al cargar el subgrupo de cuenta';
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
        this.error = err.error?.message || 'Error al guardar el subgrupo de cuenta';
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
      return 'Este campo es requerido';
    }
    if (control?.hasError('pattern')) {
      return 'Debe ser un código de 5 dígitos (ej: 01001)';
    }
    if (control?.hasError('min')) {
      return 'Debe seleccionar un grupo de cuenta';
    }
    if (control?.hasError('maxlength')) {
      const maxLength = control.errors?.['maxlength'].requiredLength;
      return `Máximo ${maxLength} caracteres`;
    }
    return '';
  }
}
