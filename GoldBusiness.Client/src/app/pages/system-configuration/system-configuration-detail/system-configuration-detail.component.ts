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
  
  // ✅ Control de error de imagen para evitar bucles
  logoError = false;

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
    this.logoError = false; // ✅ Reset del flag al recargar
    
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

  /** Devuelve la URL del logo para usar en [src] */
  getLogoUrl(): string | null {
    // ✅ Si ya hubo error, no devolver nada (mostrar placeholder en HTML)
    if (this.logoError) return null;
    
    if (!this.config?.imagen) return null;
    
    return this.systemConfigurationService.getLogoUrl(this.config.imagen);
  }

  goToEdit(): void {
    if (this.id) this.router.navigate(['/configuracion/editar', this.id]);
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

  /** ✅ Se ejecuta una sola vez cuando falla la carga del logo */
  onImageError(event: Event): void {
    console.warn('❌ Error al cargar logo, usando placeholder');
    this.logoError = true; // ✅ Marcar que hubo error para evitar bucle
  }
}
