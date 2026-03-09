import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EstablecimientoService, EstablecimientoDTO } from '../../../services/establecimiento.service';
import { LocalidadService, LocalidadDTO } from '../../../services/localidad.service';

@Component({
  selector: 'app-establecimiento-form',
  templateUrl: './establecimiento-form.component.html',
  styleUrls: ['./establecimiento-form.component.css']
})
export class EstablecimientoFormComponent implements OnInit {
  itemForm: FormGroup;
  isEditMode = false;
  itemId?: number;
  loading = false;
  saving = false;
  error: string | null = null;
  
  localidades: LocalidadDTO[] = [];

  constructor(
    private fb: FormBuilder,
    private establecimientoService: EstablecimientoService,
    private localidadService: LocalidadService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.itemForm = this.fb.group({
      codigo: ['', [Validators.required, Validators.maxLength(10)]],
      descripcion: ['', [Validators.required, Validators.maxLength(100)]], // ✅ descripcion
      telefono: ['', Validators.maxLength(20)],
      email: ['', [Validators.email, Validators.maxLength(100)]],
      direccion: ['', Validators.maxLength(200)],
      localidadId: ['']
    });
  }

  ngOnInit(): void {
    this.loadLocalidades();
    
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.itemId = +params['id'];
        this.isEditMode = true;
        this.loadItem();
      }
    });
  }

  loadLocalidades(): void {
    this.localidadService.getAll().subscribe({
      next: (localidades) => {
        this.localidades = localidades;
      },
      error: (error) => {
        console.error('Error loading localidades:', error);
        this.error = 'Error al cargar las localidades';
      }
    });
  }

  loadItem(): void {
    if (!this.itemId) return;

    this.loading = true;
    this.error = null;

    this.establecimientoService.getById(this.itemId).subscribe({
      next: (item) => {
        this.itemForm.patchValue(item);
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading establecimiento:', error);
        this.error = error.message || 'Error al cargar el establecimiento';
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

    const formData: EstablecimientoDTO = {
      ...this.itemForm.value,
      id: this.itemId
    };

    const request = this.isEditMode
      ? this.establecimientoService.update(this.itemId!, formData)
      : this.establecimientoService.create(formData);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.router.navigate(['/organizacion/establecimiento']);
      },
      error: (error) => {
        console.error('Error saving establecimiento:', error);
        this.error = error.message || 'Error al guardar el establecimiento';
        this.saving = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/organizacion/establecimiento']);
  }
}
