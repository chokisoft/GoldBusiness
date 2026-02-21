import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { SystemConfigurationService, SystemConfigurationDTO } from '../../../services/system-configuration.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-system-configuration-detail',
  templateUrl: './system-configuration-detail.component.html',
  styleUrls: ['./system-configuration-detail.component.css']
})
export class SystemConfigurationDetailComponent implements OnInit, OnDestroy {
  config: SystemConfigurationDTO | null = null;
  id: number | null = null;
  loading = true;
  error: string | null = null;

  private languageSubscription?: Subscription;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private systemConfigurationService: SystemConfigurationService,
    private languageService: LanguageService
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

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 SysConfigDetail: Idioma cambiado, recargando...');
        this.loadConfiguration();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  loadConfiguration(): void {
    if (!this.id) return;

    this.loading = true;
    this.error = null;

    this.systemConfigurationService.getById(this.id).subscribe({
      next: (data: SystemConfigurationDTO) => {
        this.config = data;
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Error al cargar la configuración';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  goToEdit(): void {
    if (this.id) {
      this.router.navigate(['/configuracion/editar', this.id]);
    }
  }

  goBack(): void {
    this.router.navigate(['/configuracion']);
  }

  getEstadoBadgeClass(): string {
    if (!this.config) return 'badge-secondary';
    if (this.config.estaVencida) return 'badge-danger';
    if (this.config.proximoAVencer) return 'badge-warning';
    return 'badge-success';
  }

  getEstadoIcon(): string {
    if (!this.config) return '❓';
    if (this.config.estaVencida) return '❌';
    if (this.config.proximoAVencer) return '⚠️';
    return '✅';
  }

  onImageError(event: Event): void {
    const imgElement = event.target as HTMLImageElement;
    imgElement.src = 'assets/placeholder-logo.png';
  }
}
