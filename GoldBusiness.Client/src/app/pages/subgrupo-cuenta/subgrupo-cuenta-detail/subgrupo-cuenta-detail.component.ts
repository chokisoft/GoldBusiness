import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { SubGrupoCuentaService, SubGrupoCuentaDTO } from '../../../services/subgrupo-cuenta.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-subgrupo-cuenta-detail',
  templateUrl: './subgrupo-cuenta-detail.component.html',
  styleUrls: ['./subgrupo-cuenta-detail.component.css']
})
export class SubGrupoCuentaDetailComponent implements OnInit, OnDestroy {
  subgrupo: SubGrupoCuentaDTO | null = null;
  id: number | null = null;
  loading = true;
  error: string | null = null;

  private languageSubscription?: Subscription;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private subGrupoCuentaService: SubGrupoCuentaService,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      this.loadSubGrupoCuenta();
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 SubGrupoDetail: Idioma cambiado, recargando...');
        this.loadSubGrupoCuenta();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  loadSubGrupoCuenta(): void {
    this.loading = true;
    this.error = null;

    this.subGrupoCuentaService.getById(this.id!).subscribe({
      next: (data) => {
        this.subgrupo = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar el subgrupo de cuenta';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  goToEdit(): void {
    this.router.navigate(['/nomencladores/subgrupo-cuenta/editar', this.id]);
  }

  goBack(): void {
    this.router.navigate(['/nomencladores/subgrupo-cuenta']);
  }

  delete(): void {
    if (!confirm(`¿Está seguro que desea eliminar el subgrupo "${this.subgrupo?.descripcion}"?`)) {
      return;
    }

    this.subGrupoCuentaService.delete(this.id!).subscribe({
      next: () => {
        this.router.navigate(['/nomencladores/subgrupo-cuenta']);
      },
      error: (err) => {
        this.error = err.error?.message || 'Error al eliminar el subgrupo de cuenta';
        console.error('Error:', err);
      }
    });
  }
}
