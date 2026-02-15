import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { GrupoCuentaService, GrupoCuentaDTO } from '../../../services/grupo-cuenta.service';

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
    private route: ActivatedRoute
  ) {
    this.form = this.fb.group({
      codigo: ['', [Validators.required, Validators.maxLength(10)]],
      nombre: ['', [Validators.required, Validators.maxLength(100)]],
      descripcion: ['', Validators.maxLength(500)],
      activo: [true]
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.grupoId = +id;
      this.loadGrupoCuenta();
    }
  }

  loadGrupoCuenta(): void {
    if (!this.grupoId) return;

    this.loading = true;
    this.grupoCuentaService.getById(this.grupoId).subscribe({
      next: (data) => {
        this.form.patchValue(data);
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar el grupo de cuenta';
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

    const dto: GrupoCuentaDTO = this.form.value;

    const operation = this.isEditMode
      ? this.grupoCuentaService.update(this.grupoId!, dto)
      : this.grupoCuentaService.create(dto);

    operation.subscribe({
      next: () => {
        this.router.navigate(['/nomencladores/grupo-cuenta']);
      },
      error: (err) => {
        this.error = err.error?.message || 'Error al guardar el grupo de cuenta';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/nomencladores/grupo-cuenta']);
  }

  getErrorMessage(fieldName: string): string {
    const control = this.form.get(fieldName);
    if (control?.hasError('required')) {
      return 'Este campo es requerido';
    }
    if (control?.hasError('maxlength')) {
      const maxLength = control.errors?.['maxlength'].requiredLength;
      return `Máximo ${maxLength} caracteres`;
    }
    return '';
  }
}
