import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { CuentaService, CuentaDTO } from '../../../services/cuenta.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-cuenta-detail',
  templateUrl: './cuenta-detail.component.html',
  styleUrls: ['./cuenta-detail.component.css']
})
export class CuentaDetailComponent implements OnInit, OnDestroy {
  cuenta: CuentaDTO | null = null;
  id: number | null = null;
  loading = true;
  error: string | null = null;

  private languageSubscription?: Subscription;

  constructor(
    private cuentaService: CuentaService,
    private route: ActivatedRoute,
    private router: Router,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      this.loadCuenta();
    } else {
      this.error = 'ID no válido';
      this.loading = false;
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 CuentaDetail: Idioma cambiado, recargando...');
        this.loadCuenta();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  loadCuenta(): void {
    if (!this.id) return;

    this.loading = true;
    this.cuentaService.getById(this.id).subscribe({
      next: (data) => {
        this.cuenta = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar la cuenta';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/nomencladores/cuenta']);
  }
}
