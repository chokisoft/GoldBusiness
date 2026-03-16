import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Proveedor, ProveedorService } from '../../../services/proveedor.service';

@Component({
  selector: 'app-proveedor-form',
  templateUrl: './proveedor-form.component.html',
  styleUrls: ['./proveedor-form.component.css']
})
export class ProveedorFormComponent implements OnInit {
  itemForm: FormGroup;
  isEditMode = false;
  itemId?: number;
  loading = false;
  saving = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private proveedorService: ProveedorService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.itemForm = this.fb.group({
      codigo: ['', [Validators.required, Validators.maxLength(20)]],
      descripcion: ['', [Validators.required, Validators.maxLength(120)]], // antes: nombre
      nif: ['', Validators.maxLength(30)], // antes: rfc
      telefono1: ['', Validators.maxLength(20)], // antes: telefono
      email1: ['', [Validators.email, Validators.maxLength(100)]], // antes: email
      direccion: ['', Validators.maxLength(200)]
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

    this.proveedorService.getById(this.itemId).subscribe({
      next: (item) => {
        this.itemForm.patchValue(item);
        this.loading = false;
      },
      error: (error) => {
        this.error = error.message || 'Error al cargar el proveedor';
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

    const formData: Proveedor = {
      ...this.itemForm.value,
      id: this.itemId
    };

    const request = this.isEditMode
      ? this.proveedorService.update(this.itemId!, formData)
      : this.proveedorService.create(formData);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.router.navigate(['/nomencladores/proveedores']);
      },
      error: (error) => {
        this.error = error.message || 'Error al guardar el proveedor';
        this.saving = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/nomencladores/proveedores']);
  }
}
