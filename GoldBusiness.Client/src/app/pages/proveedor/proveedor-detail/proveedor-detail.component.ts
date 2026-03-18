import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ProveedorDTO, ProveedorService } from '../../../services/proveedor.service';
import { PaisService, PaisDTO } from '../../../services/pais.service';

@Component({
  selector: 'app-proveedor-detail',
  templateUrl: './proveedor-detail.component.html',
  styleUrls: ['./proveedor-detail.component.css']
})
export class ProveedorDetailComponent implements OnInit, OnDestroy {
  item?: ProveedorDTO;
  loading = false;
  error: string | null = null;

  selectedPais?: PaisDTO;
  paisDescripcion?: string;
  postalCode?: string;
  private sub?: Subscription;
  private paisSub?: Subscription;

  constructor(
    private proveedorService: ProveedorService,
    private route: ActivatedRoute,
    private router: Router,
    private paisService: PaisService
  ) {}

  ngOnInit(): void {
    this.sub = this.route.params.subscribe(params => {
      const id = +params['id'];
      if (id) this.loadItem(id);
    });
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
    this.paisSub?.unsubscribe();
  }

  loadItem(id: number): void {
    this.loading = true;
    this.error = null;
    this.selectedPais = undefined;
    this.paisDescripcion = undefined;
    this.postalCode = undefined;
    this.paisSub?.unsubscribe();

    this.proveedorService.getById(id).subscribe({
      next: item => {
        this.item = item;

        this.paisDescripcion = (item as any)?.paisDescripcion ?? undefined;
        this.postalCode = (item as any)?.codigoPostalCodigo ?? (item as any)?.codPostal ?? undefined;

        const paisId = (item as any)?.paisId;
        if (paisId) {
          this.paisSub = this.paisService.getById(paisId).subscribe({
            next: p => {
              this.selectedPais = p;
              this.paisDescripcion = p?.descripcion ?? this.paisDescripcion;
            },
            error: () => {
              this.selectedPais = undefined;
            }
          });
        }

        this.loading = false;
      },
      error: err => {
        this.error = err?.message || 'Error al cargar el proveedor';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/nomencladores/proveedores']);
  }
}
