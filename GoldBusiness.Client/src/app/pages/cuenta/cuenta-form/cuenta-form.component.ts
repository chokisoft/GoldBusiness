import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CuentaService, CuentaDTO } from '../../../services/cuenta.service';
import { SubGrupoCuentaService, SubGrupoCuentaDTO } from '../../../services/subgrupo-cuenta.service';

@Component({
  selector: 'app-cuenta-form',
  templateUrl: './cuenta-form.component.html',
  styleUrls: ['./cuenta-form.component.css']
})
export class CuentaFormComponent implements OnInit {
  form: FormGroup;
  subgrupos: SubGrupoCuentaDTO[] = [];
  isEditMode = false;
  cuentaId: number | null = null;
  loading = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private cuentaService: CuentaService,
    private subGrupoCuentaService: SubGrupoCuentaService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.form = this.fb.group({
      codigo: ['', [Validators.required, Validators.pattern(/^\d{8}$/)]],
      subGrupoCuentaId: [null, [Validators.required, Validators.min(1)]],
      descripcion: ['', [Validators.required, Validators.maxLength(256)]]
    });
  }

  ngOnInit(): void {
    this.loadSubGrupos();
    
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.cuentaId = +id;
      this.loadCuenta();
    }
  }

  loadSubGrupos(): void {
    this.subGrupoCuentaService.getAll().subscribe({
      next: (data) => {
        this.subgrupos = data.filter(sg => !sg.cancelado);
      },
      error: (err) => {
        console.error('Error al cargar subgrupos:', err);
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

    const dto: CuentaDTO = this.form.value;

    const operation = this.isEditMode
      ? this.cuentaService.update(this.cuentaId!, dto)
      : this.cuentaService.create(dto);

    operation.subscribe({
      next: () => {
        this.router.navigate(['/nomencladores/cuenta']);
      },
      error: (err) => {
        this.error = err.error?.message || 'Error al guardar la cuenta';
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
      return 'Este campo es requerido';
    }
    if (control?.hasError('pattern')) {
      return 'Debe ser un código de 8 dígitos (ej: 10000010)';
    }
    if (control?.hasError('min')) {
      return 'Debe seleccionar un subgrupo de cuenta';
    }
    if (control?.hasError('maxlength')) {
      const maxLength = control.errors?.['maxlength'].requiredLength;
      return `Máximo ${maxLength} caracteres`;
    }
    return '';
  }
}
