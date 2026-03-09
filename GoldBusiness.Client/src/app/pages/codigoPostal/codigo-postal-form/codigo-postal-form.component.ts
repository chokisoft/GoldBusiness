import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CodigoPostalService, CodigoPostalDTO } from '../../../services/codigo-postal.service';
import { PaisService, PaisDTO } from '../../../services/pais.service';
import { ProvinciaService, ProvinciaDTO } from '../../../services/provincia.service';
import { MunicipioService, MunicipioDTO } from '../../../services/municipio.service';

@Component({
  selector: 'app-codigo-postal-form',
  templateUrl: './codigo-postal-form.component.html',
  styleUrls: ['./codigo-postal-form.component.css']
})
export class CodigoPostalFormComponent implements OnInit {
  itemForm: FormGroup;
  isEditMode = false;
  itemId?: number;
  loading = false;
  saving = false;
  error: string | null = null;
  
  // Cascada de selects
  paises: PaisDTO[] = [];
  provincias: ProvinciaDTO[] = [];
  municipios: MunicipioDTO[] = [];
  
  // Estados de carga
  loadingPaises = false;
  loadingProvincias = false;
  loadingMunicipios = false;

  constructor(
    private fb: FormBuilder,
    private codigoPostalService: CodigoPostalService,
    private paisService: PaisService,
    private provinciaService: ProvinciaService,
    private municipioService: MunicipioService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.itemForm = this.fb.group({
      paisId: ['', Validators.required],
      provinciaId: ['', Validators.required],
      municipioId: ['', Validators.required],
      codigo: ['', [Validators.required, Validators.maxLength(20)]],
      descripcion: ['', Validators.maxLength(500)]
    });
  }

  ngOnInit(): void {
    this.loadPaises();
    
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.itemId = +params['id'];
        this.isEditMode = true;
        this.loadItem();
      }
    });
  }

  loadPaises(): void {
    this.loadingPaises = true;
    this.paisService.getAll().subscribe({
      next: (paises) => {
        this.paises = paises;
        this.loadingPaises = false;
      },
      error: (error) => {
        console.error('Error loading países:', error);
        this.error = 'Error al cargar los países';
        this.loadingPaises = false;
      }
    });
  }

  onPaisChange(): void {
    const paisId = this.itemForm.get('paisId')?.value;
    
    // Resetear provincias y municipios
    this.provincias = [];
    this.municipios = [];
    this.itemForm.patchValue({ provinciaId: '', municipioId: '' });
    
    if (paisId) {
      this.loadProvincias(paisId);
    }
  }

  loadProvincias(paisId: number): void {
    this.loadingProvincias = true;
    this.provinciaService.getByPaisId(paisId).subscribe({
      next: (provincias) => {
        this.provincias = provincias;
        this.loadingProvincias = false;
      },
      error: (error) => {
        console.error('Error loading provincias:', error);
        this.provincias = [];
        this.loadingProvincias = false;
      }
    });
  }

  onProvinciaChange(): void {
    const provinciaId = this.itemForm.get('provinciaId')?.value;
    
    // Resetear municipios
    this.municipios = [];
    this.itemForm.patchValue({ municipioId: '' });
    
    if (provinciaId) {
      this.loadMunicipios(provinciaId);
    }
  }

  loadMunicipios(provinciaId: number): void {
    this.loadingMunicipios = true;
    this.municipioService.getByProvinciaId(provinciaId).subscribe({
      next: (municipios) => {
        this.municipios = municipios;
        this.loadingMunicipios = false;
      },
      error: (error) => {
        console.error('Error loading municipios:', error);
        this.municipios = [];
        this.loadingMunicipios = false;
      }
    });
  }

  loadItem(): void {
    if (!this.itemId) return;

    this.loading = true;
    this.error = null;

    this.codigoPostalService.getById(this.itemId).subscribe({
      next: (item) => {
        // Cargar la cascada primero
        if (item.municipio?.provincia?.paisId) {
          this.loadProvincias(item.municipio.provincia.paisId);
          setTimeout(() => {
            if (item.municipio?.provinciaId) {
              this.loadMunicipios(item.municipio.provinciaId);
            }
          }, 200);
        }
        
        // Establecer valores del formulario
        setTimeout(() => {
          this.itemForm.patchValue({
            paisId: item.municipio?.provincia?.paisId || '',
            provinciaId: item.municipio?.provinciaId || '',
            municipioId: item.municipioId,
            codigo: item.codigo,
            descripcion: item.descripcion || ''
          });
        }, 400);
        
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading código postal:', error);
        this.error = error.message || 'Error al cargar el código postal';
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

    const formData: CodigoPostalDTO = {
      id: this.itemId,
      codigo: this.itemForm.value.codigo,
      municipioId: this.itemForm.value.municipioId,
      descripcion: this.itemForm.value.descripcion || undefined
    };

    const request = this.isEditMode
      ? this.codigoPostalService.update(this.itemId!, formData)
      : this.codigoPostalService.create(formData);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.router.navigate(['/organizacion/codigo-postal']);
      },
      error: (error) => {
        console.error('Error saving código postal:', error);
        this.error = error.message || 'Error al guardar el código postal';
        this.saving = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/organizacion/codigo-postal']);
  }
}
