import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LocalidadService, LocalidadDTO } from '../../../services/localidad.service';
import { EstablecimientoDTO, EstablecimientoService } from '../../../services/establecimiento.service';

@Component({
  selector: 'app-localidad-form',
  templateUrl: './localidad-form.component.html',
  styleUrls: ['./localidad-form.component.css']
})
export class LocalidadFormComponent implements OnInit {
  itemForm: FormGroup;
  isEditMode = false;
  itemId?: number;
  loading = false;
  saving = false;
  error: string | null = null;
  
  establecimientos: EstablecimientoDTO[] = [];

  constructor(
    private fb: FormBuilder,
    private localidadService: LocalidadService,
    private establecimientoService: EstablecimientoService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.itemForm = this.fb.group({
      establecimientoId: ['', Validators.required],
      codigo: ['', [Validators.required, Validators.maxLength(10)]],
      descripcion: ['', [Validators.required, Validators.maxLength(100)]], // ✅ descripcion
      almacen: [false]
    });
  }

  ngOnInit(): void {
    this.loadEstablecimientos();
    
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.itemId = +params['id'];
        this.isEditMode = true;
        this.loadItem();
      }
    });
  }

  loadEstablecimientos(): void {
    this.establecimientoService.getAll().subscribe({
      next: (establecimientos) => {
        this.establecimientos = establecimientos;
      },
      error: (error) => {
        console.error('Error loading establecimientos:', error);
        this.error = 'Error al cargar los establecimientos';
      }
    });
  }

  loadItem(): void {
    if (!this.itemId) return;

    this.loading = true;
    this.error = null;

    this.localidadService.getById(this.itemId).subscribe({
      next: (item) => {
        this.itemForm.patchValue(item);
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading localidad:', error);
        this.error = error.message || 'Error al cargar la localidad';
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

    const formData: LocalidadDTO = {
      ...this.itemForm.value,
      id: this.itemId
    };

    const request = this.isEditMode
      ? this.localidadService.update(this.itemId!, formData)
      : this.localidadService.create(formData);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.router.navigate(['/organizacion/localidad']);
      },
      error: (error) => {
        console.error('Error saving localidad:', error);
        this.error = error.message || 'Error al guardar la localidad';
        this.saving = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/organizacion/localidad']);
  }
}
