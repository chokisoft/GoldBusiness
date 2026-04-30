import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { LocalidadService, LocalidadDTO, TipoLocalidad } from '../../../services/localidad.service';

@Component({
  selector: 'app-localidad-detail',
  templateUrl: './localidad-detail.component.html',
  styleUrls: ['./localidad-detail.component.css']
})
export class LocalidadDetailComponent implements OnInit {
  item?: LocalidadDTO;
  loading = false;
  error: string | null = null;

  // Referencia al enum para usar en template
  TipoLocalidad = TipoLocalidad;

  constructor(
    private localidadService: LocalidadService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = +params['id'];
      if (id) {
        this.loadItem(id);
      }
    });
  }

  loadItem(id: number): void {
    this.loading = true;
    this.error = null;

    this.localidadService.getById(id).subscribe({
      next: (item) => {
        this.item = item;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading localidad:', error);
        this.error = error.message || 'Error al cargar la localidad';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.location.back();
  }

  /**
   * Obtiene el label legible del tipo de localidad
   */
  getTipoLabel(tipo: TipoLocalidad): string {
    return this.localidadService.getTipoLocalidadLabel(tipo);
  }

  /**
   * Obtiene el icono para el tipo de localidad
   */
  getTipoIcon(tipo: TipoLocalidad): string {
    switch (tipo) {
      case TipoLocalidad.Almacen: return '📦';
      case TipoLocalidad.PuntoVenta: return '🛒';
      case TipoLocalidad.AreaRecepcion: return '📥';
      case TipoLocalidad.AreaDespacho: return '📤';
      case TipoLocalidad.Produccion: return '🏭';
      case TipoLocalidad.Transito: return '🚚';
      case TipoLocalidad.Cuarentena: return '🔒';
      case TipoLocalidad.Consignacion: return '🤝';
      case TipoLocalidad.Devoluciones: return '↩️';
      case TipoLocalidad.Obsoletos: return '♻️';
      default: return '❓';
    }
  }
}
