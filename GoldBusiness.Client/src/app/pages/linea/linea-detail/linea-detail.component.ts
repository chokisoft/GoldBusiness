import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { LineaService, LineaDTO } from '../../../services/linea.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-linea-detail',
  templateUrl: './linea-detail.component.html',
  styleUrls: ['./linea-detail.component.css']
})
export class LineaDetailComponent implements OnInit, OnDestroy {
  linea: LineaDTO | null = null;
  id: number | null = null;
  loading = true;
  error: string | null = null;

  private languageSubscription?: Subscription;

  constructor(
    private lineaService: LineaService,
    private route: ActivatedRoute,
    private router: Router,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      this.loadLinea();
    } else {
      this.error = 'ID no válido';
      this.loading = false;
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        this.loadLinea();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  loadLinea(): void {
    if (!this.id) return;

    this.loading = true;
    this.lineaService.getById(this.id).subscribe({
      next: (data) => {
        this.linea = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar el linea';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/nomencladores/linea']);
  }
}
