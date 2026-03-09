import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LocalidadService, LocalidadDTO } from '../../../services/localidad.service';
import { MunicipioService, MunicipioDTO } from '../../../services/municipio.service';

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
  
  municipios: MunicipioDTO[] = [];

  constructor(
    private fb: FormBuilder,
    private localidadService: LocalidadService,
    private municipioService: MunicipioService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.itemForm = this.fb.group({
      municipioId: ['', Validators.required],
      codigo: ['', [Validators.required, Validators.maxLength(10)]],
      descripcion: ['', [Validators.required, Validators.maxLength(100)]] // ✅ descripcion
    });
  }

  ngOnInit(): void {
    this.loadMunicipios();
    
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.itemId = +params['id'];
        this.isEditMode = true;
        this.loadItem();
      }
    });
  }

  loadMunicipios(): void {
    this.municipioService.getAll().subscribe({
      next: (municipios) => {
        this.municipios = municipios;
      },
      error: (error) => {
        console.error('Error loading municipios:', error);
        this.error = 'Error al cargar los municipios';
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
