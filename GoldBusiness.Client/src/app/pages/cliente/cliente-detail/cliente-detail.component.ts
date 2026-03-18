import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ClienteDTO, ClienteService } from '../../../services/cliente.service';
import { PaisService, PaisDTO } from '../../../services/pais.service';

@Component({
  selector: 'app-cliente-detail',
  templateUrl: './cliente-detail.component.html',
  styleUrls: ['./cliente-detail.component.css']
})
export class ClienteDetailComponent implements OnInit, OnDestroy {
  item?: ClienteDTO;
  loading = false;
  error: string | null = null;

  selectedPais?: PaisDTO;
  paisDescripcion?: string;
  postalCode?: string;
  private sub?: Subscription;
  private paisSub?: Subscription;

  constructor(
    private clienteService: ClienteService,
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

    this.clienteService.getById(id).subscribe({
      next: item => {
        this.item = item;

        // fallbacks from DTO (if backend returned them)
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
        this.error = err?.message || 'Error al cargar el cliente';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/nomencladores/clientes']);
  }
}
