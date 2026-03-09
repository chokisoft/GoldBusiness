import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { ConceptoAjusteService, ConceptoAjusteDTO } from '../../../services/concepto-ajuste.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-concepto-ajuste-detail',
  templateUrl: './concepto-ajuste-detail.component.html',
  styleUrls: ['./concepto-ajuste-detail.component.css']
})
export class ConceptoAjusteDetailComponent implements OnInit, OnDestroy {
  conceptoAjuste: ConceptoAjusteDTO | null = null;
  id: number | null = null;
  loading = true;
  error: string | null = null;

  private languageSubscription?: Subscription;

  constructor(
    private conceptoAjusteService: ConceptoAjusteService,
    private route: ActivatedRoute,
    private router: Router,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      this.loadConceptoAjuste();
    } else {
      this.error = 'ID no válido';
      this.loading = false;
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        this.loadConceptoAjuste();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  loadConceptoAjuste(): void {
    if (!this.id) return;

    this.loading = true;
    this.conceptoAjusteService.getById(this.id).subscribe({
      next: (data) => {
        this.conceptoAjuste = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar el conceptoAjuste';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/nomencladores/concepto-ajuste']);
  }
}
