import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { SubLineaService, SubLineaDTO } from '../../../services/sub-linea.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-sub-linea-detail',
  templateUrl: './sub-linea-detail.component.html',
  styleUrls: ['./sub-linea-detail.component.css']
})
export class SubLineaDetailComponent implements OnInit, OnDestroy {
  subLinea: SubLineaDTO | null = null;
  id: number | null = null;
  loading = true;
  error: string | null = null;

  private languageSubscription?: Subscription;

  constructor(
    private subLineaService: SubLineaService,
    private route: ActivatedRoute,
    private router: Router,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      this.loadSubLinea();
    } else {
      this.error = 'ID no válido';
      this.loading = false;
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        this.loadSubLinea();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  loadSubLinea(): void {
    if (!this.id) return;

    this.loading = true;
    this.subLineaService.getById(this.id).subscribe({
      next: (data) => {
        this.subLinea = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar el subLinea';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/nomencladores/sub-linea']);
  }
}
