import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { UnidadMedidaService, UnidadMedidaDTO } from '../../../services/unidad-medida.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-unidad-medida-detail',
  templateUrl: './unidad-medida-detail.component.html',
  styleUrls: ['./unidad-medida-detail.component.css']
})
export class UnidadMedidaDetailComponent implements OnInit, OnDestroy {
  unidadMedida: UnidadMedidaDTO | null = null;
  id: number | null = null;
  loading = true;
  error: string | null = null;

  private languageSubscription?: Subscription;

  constructor(
    private unidadMedidaService: UnidadMedidaService,
    private route: ActivatedRoute,
    private router: Router,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      this.loadUnidadMedida();
    } else {
      this.error = 'ID no válido';
      this.loading = false;
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        this.loadUnidadMedida();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  loadUnidadMedida(): void {
    if (!this.id) return;

    this.loading = true;
    this.unidadMedidaService.getById(this.id).subscribe({
      next: (data) => {
        this.unidadMedida = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar el unidadMedida';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/nomencladores/unidad-medida']);
  }
}
