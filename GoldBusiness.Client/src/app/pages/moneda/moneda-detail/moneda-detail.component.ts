import { Component, OnDestroy, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { MonedaService, MonedaDTO } from '../../../services/moneda.service';
import { LanguageService } from '../../../services/language.service';
import { TranslationService } from '../../../services/translation.service';

@Component({
    selector: 'app-moneda-detail',
    templateUrl: './moneda-detail.component.html',
    styleUrls: ['./moneda-detail.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class MonedaDetailComponent implements OnInit, OnDestroy {
  item?: MonedaDTO;
  loading = false;
  error: string | null = null;
  private routeSubscription?: Subscription;
  private languageSubscription?: Subscription;

  constructor(
    private monedaService: MonedaService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private languageService: LanguageService,
    private translationService: TranslationService
  ) {}

  ngOnInit(): void {
    this.routeSubscription = this.route.params.subscribe(params => {
      const id = +params['id'];
      if (id) {
        this.loadItem(id);
      }
    });

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        const id = Number(this.route.snapshot.paramMap.get('id'));
        if (id) {
          this.loadItem(id);
        }
      });
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.languageSubscription?.unsubscribe();
  }

  loadItem(id: number): void {
    this.loading = true;
    this.error = null;

    this.monedaService.getById(id).subscribe({
      next: (item) => {
        this.item = item;
        this.loading = false;
      },
      error: (error) => {
        this.error = error.message || this.translationService.translate('error.loading');
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.location.back();
  }
}
