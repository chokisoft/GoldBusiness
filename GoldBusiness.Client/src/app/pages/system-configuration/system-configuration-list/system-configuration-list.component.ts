import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SystemConfigurationService, SystemConfigurationDTO } from '../../../services/system-configuration.service';

@Component({
  selector: 'app-system-configuration-list',
  templateUrl: './system-configuration-list.component.html',
  styleUrls: ['./system-configuration-list.component.css']
})
export class SystemConfigurationListComponent implements OnInit {
  configurations: SystemConfigurationDTO[] = [];
  loading = true;
  error: string | null = null;

  constructor(
    private systemConfigurationService: SystemConfigurationService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadConfigurations();
  }

  loadConfigurations(): void {
    this.loading = true;
    this.error = null;

    this.systemConfigurationService.getAll().subscribe({
      next: (data) => {
        // ✅ CAMBIO: key → codigoSistema
        this.configurations = data.sort((a, b) => 
          a.codigoSistema.localeCompare(b.codigoSistema)
        );
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar las configuraciones';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  getEstadoBadgeClass(estadoLicencia: string | undefined): string {
    if (!estadoLicencia) return 'badge-secondary';

    switch (estadoLicencia.toLowerCase()) {
      case 'vigente':
        return 'badge-success';
      case 'por vencer':
        return 'badge-warning';
      case 'vencida':
        return 'badge-danger';
      default:
        return 'badge-secondary';
    }
  }

  getEstadoIcon(estadoLicencia: string | undefined): string {
    if (!estadoLicencia) return '❓';

    switch (estadoLicencia.toLowerCase()) {
      case 'vigente':
        return '✅';
      case 'por vencer':
        return '⚠️';
      case 'vencida':
        return '❌';
      default:
        return '❓';
    }
  }
}
