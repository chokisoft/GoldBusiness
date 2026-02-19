import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SystemConfigurationService, SystemConfigurationDTO } from '../../../services/system-configuration.service';

@Component({
  selector: 'app-system-configuration-detail',
  templateUrl: './system-configuration-detail.component.html',
  styleUrls: ['./system-configuration-detail.component.css']
})
export class SystemConfigurationDetailComponent implements OnInit {
  config: SystemConfigurationDTO | null = null; // ✅ CORRECTO
  id: number | null = null;
  loading = true;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private systemConfigurationService: SystemConfigurationService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      this.loadConfiguration();
    } else {
      this.error = 'ID no válido';
      this.loading = false;
    }
  }

  loadConfiguration(): void {
    if (!this.id) return; // ✅ CAMBIO: configId → id

    this.loading = true;
    this.error = null;

    this.systemConfigurationService.getById(this.id).subscribe({ // ✅ CAMBIO: configId → id
      next: (data: SystemConfigurationDTO) => { // ✅ AGREGAR tipo
        this.config = data; // ✅ CAMBIO: configuration → config
        this.loading = false;
      },
      error: (err: any) => { // ✅ AGREGAR tipo 'any'
        this.error = 'Error al cargar la configuración';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  goToEdit(): void {
    if (this.id) { // ✅ CAMBIO: configId → id
      this.router.navigate(['/configuracion/editar', this.id]);
    }
  }

  goBack(): void {
    this.router.navigate(['/configuracion']);
  }

  getEstadoBadgeClass(): string {
    if (!this.config) return 'badge-secondary'; // ✅ CAMBIO: configuration → config

    if (this.config.estaVencida) return 'badge-danger'; // ✅ CAMBIO
    if (this.config.proximoAVencer) return 'badge-warning'; // ✅ CAMBIO
    return 'badge-success';
  }

  getEstadoIcon(): string {
    if (!this.config) return '❓'; // ✅ CAMBIO: configuration → config

    if (this.config.estaVencida) return '❌'; // ✅ CAMBIO
    if (this.config.proximoAVencer) return '⚠️'; // ✅ CAMBIO
    return '✅';
  }

  onImageError(event: Event): void {
    const imgElement = event.target as HTMLImageElement;
    imgElement.src = 'assets/placeholder-logo.png';
  }
}
