import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductoDTO, ProductoService } from '../../../services/producto.service';

@Component({
  selector: 'app-producto-form',
  templateUrl: './producto-form.component.html',
  styleUrl: './producto-form.component.css'
})
export class ProductoFormComponent implements OnInit {
  itemForm: FormGroup;
  isEditMode = false;
  itemId?: number;
  loading = false;
  saving = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private productoService: ProductoService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.itemForm = this.fb.group({
      codigo: ['', [Validators.required, Validators.maxLength(20)]],
      nombre: ['', [Validators.required, Validators.maxLength(120)]],
      descripcion: ['', Validators.maxLength(200)],
      precio: [0, [Validators.min(0)]],
      costo: [0, [Validators.min(0)]],
      stock: [0, [Validators.min(0)]],
      unidadMedida: ['', Validators.maxLength(50)],
      linea: ['', Validators.maxLength(50)],
      sublinea: ['', Validators.maxLength(50)]
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

    this.productoService.getById(this.itemId).subscribe({
      next: (item) => {
        this.itemForm.patchValue(item);
        this.loading = false;
      },
      error: (error) => {
        this.error = error.message || 'Error al cargar el producto';
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

    const formData: ProductoDTO = {
      ...this.itemForm.value,
      id: this.itemId
    };

    const request = this.isEditMode
      ? this.productoService.update(this.itemId!, formData)
      : this.productoService.create(formData);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.router.navigate(['/nomencladores/producto']);
      },
      error: (error) => {
        this.error = error.message || 'Error al guardar el producto';
        this.saving = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/nomencladores/producto']);
  }

}
