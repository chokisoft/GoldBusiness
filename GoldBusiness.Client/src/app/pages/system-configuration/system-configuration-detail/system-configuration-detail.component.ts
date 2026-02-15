import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SystemConfigurationService, SystemConfigurationDTO } from '../../../services/system-configuration.service';

@Component({
  selector: 'app-system-configuration-detail',
  templateUrl: './system-configuration-detail.component.html',
  styleUrls: ['./system-configuration-detail.component.css']
})
export class SystemConfigurationDetailComponent implements OnInit {
  configuration: SystemConfigurationDTO | null = null;
  loading = true;
  error: string | null = null;
  configId: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private systemConfigurationService: SystemConfigurationService
  ) {
    this.configId = +this.route.snapshot.paramMap.get('id')!;
  }

  ngOnInit(): void {
    this.loadConfiguration();
  }

  loadConfiguration(): void {
    this.loading = true;
    this.error = null;

    this.systemConfigurationService.getById(this.configId).subscribe({
      next: (data) => {
        this.configuration = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar la configuración';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  goToEdit(): void {
    this.router.navigate(['/configuracion/editar', this.configId]);
  }

  goBack(): void {
    this.router.navigate(['/configuracion']);
  }

  getEstadoBadgeClass(): string {
    if (!this.configuration) return 'badge-secondary';

    if (this.configuration.estaVencida) return 'badge-danger';
    if (this.configuration.proximoAVencer) return 'badge-warning';
    return 'badge-success';
  }

  getEstadoIcon(): string {
    if (!this.configuration) return '❓';

    if (this.configuration.estaVencida) return '❌';
    if (this.configuration.proximoAVencer) return '⚠️';
    return '✅';
  }

  onImageError(event: Event): void {
    const imgElement = event.target as HTMLImageElement;
    imgElement.src = 'assets/placeholder-logo.png';
  }
}
