import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProvinciaService, ProvinciaDTO } from '../../../services/provincia.service';
import { PaisService, PaisDTO } from '../../../services/pais.service';

@Component({
  selector: 'app-provincia-form',
  templateUrl: './provincia-form.component.html',
  styleUrls: ['./provincia-form.component.css']
})
export class ProvinciaFormComponent implements OnInit {
  itemForm: FormGroup;
  isEditMode = false;
  itemId?: number;
  loading = false;
  saving = false;
  error: string | null = null;

  paises: PaisDTO[] = [];

  constructor(
    private fb: FormBuilder,
    private provinciaService: ProvinciaService,
    private paisService: PaisService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.itemForm = this.fb.group({
      paisId: ['', Validators.required],
      codigo: ['', [Validators.required, Validators.maxLength(10)]],
      descripcion: ['', [Validators.required, Validators.maxLength(100)]] // ✅ CAMBIO: descripcion
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
    this.paisService.getAll().subscribe({
      next: (paises) => {
        this.paises = paises;
      },
      error: (error) => {
        console.error('Error loading países:', error);
        this.error = 'Error al cargar los países';
      }
    });
  }

  loadItem(): void {
    if (!this.itemId) return;

    this.loading = true;
    this.error = null;

    this.provinciaService.getById(this.itemId).subscribe({
      next: (item) => {
        this.itemForm.patchValue(item);
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading provincia:', error);
        this.error = error.message || 'Error al cargar la provincia';
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

    const formData: ProvinciaDTO = {
      ...this.itemForm.value,
      id: this.itemId
    };

    const request = this.isEditMode
      ? this.provinciaService.update(this.itemId!, formData)
      : this.provinciaService.create(formData);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.router.navigate(['/organizacion/provincia']);
      },
      error: (error) => {
        console.error('Error saving provincia:', error);
        this.error = error.message || 'Error al guardar la provincia';
        this.saving = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/organizacion/provincia']);
  }
}
