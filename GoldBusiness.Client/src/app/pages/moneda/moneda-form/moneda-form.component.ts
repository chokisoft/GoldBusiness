import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MonedaService, MonedaDTO } from '../../../services/moneda.service';

@Component({
  selector: 'app-moneda-form',
  templateUrl: './moneda-form.component.html',
  styleUrls: ['./moneda-form.component.css']
})
export class MonedaFormComponent implements OnInit {
  itemForm: FormGroup;
  isEditMode = false;
  itemId?: number;
  loading = false;
  saving = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private monedaService: MonedaService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.itemForm = this.fb.group({
      codigo: ['', [Validators.required, Validators.maxLength(3)]],
      descripcion: ['', [Validators.required, Validators.maxLength(100)]], // ✅ descripcion
      simbolo: ['', Validators.maxLength(10)],
      cambio: [null, [Validators.min(0)]]
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.itemId = +params['id'];
        this.isEditMode = true;
        this.loadItem();
      }
    });
  }

  loadItem(): void {
    if (!this.itemId) return;

    this.loading = true;
    this.error = null;

    this.monedaService.getById(this.itemId).subscribe({
      next: (item) => {
        this.itemForm.patchValue(item);
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading moneda:', error);
        this.error = error.message || 'Error al cargar la moneda';
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.itemForm.invalid) {
      this.itemForm.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.error = null;

    const formData: MonedaDTO = {
      ...this.itemForm.value,
      id: this.itemId
    };

    const request = this.isEditMode
      ? this.monedaService.update(this.itemId!, formData)
      : this.monedaService.create(formData);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.router.navigate(['/organizacion/moneda']);
      },
      error: (error) => {
        console.error('Error saving moneda:', error);
        this.error = error.message || 'Error al guardar la moneda';
        this.saving = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/organizacion/moneda']);
  }
}
