import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MunicipioService, MunicipioDTO } from '../../../services/municipio.service';
import { ProvinciaService, ProvinciaDTO } from '../../../services/provincia.service';

@Component({
  selector: 'app-municipio-form',
  templateUrl: './municipio-form.component.html',
  styleUrls: ['./municipio-form.component.css']
})
export class MunicipioFormComponent implements OnInit {
  itemForm: FormGroup;
  isEditMode = false;
  itemId?: number;
  loading = false;
  saving = false;
  error: string | null = null;
  
  provincias: ProvinciaDTO[] = [];

  constructor(
    private fb: FormBuilder,
    private municipioService: MunicipioService,
    private provinciaService: ProvinciaService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.itemForm = this.fb.group({
      provinciaId: ['', Validators.required],
      codigo: ['', [Validators.required, Validators.maxLength(10)]],
      descripcion: ['', [Validators.required, Validators.maxLength(100)]] // ✅ descripcion
    });
  }

  ngOnInit(): void {
    this.loadProvincias();
    
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.itemId = +params['id'];
        this.isEditMode = true;
        this.loadItem();
      }
    });
  }

  loadProvincias(): void {
    this.provinciaService.getAll().subscribe({
      next: (provincias) => {
        this.provincias = provincias;
      },
      error: (error) => {
        console.error('Error loading provincias:', error);
        this.error = 'Error al cargar las provincias';
      }
    });
  }

  loadItem(): void {
    if (!this.itemId) return;

    this.loading = true;
    this.error = null;

    this.municipioService.getById(this.itemId).subscribe({
      next: (item) => {
        this.itemForm.patchValue(item);
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading municipio:', error);
        this.error = error.message || 'Error al cargar el municipio';
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

    const formData: MunicipioDTO = {
      ...this.itemForm.value,
      id: this.itemId
    };

    const request = this.isEditMode
      ? this.municipioService.update(this.itemId!, formData)
      : this.municipioService.create(formData);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.router.navigate(['/organizacion/municipio']);
      },
      error: (error) => {
        console.error('Error saving municipio:', error);
        this.error = error.message || 'Error al guardar el municipio';
        this.saving = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/organizacion/municipio']);
  }
}
