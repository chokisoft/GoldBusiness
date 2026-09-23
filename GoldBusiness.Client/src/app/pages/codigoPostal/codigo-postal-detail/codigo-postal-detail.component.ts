import { Component, OnDestroy, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { CodigoPostalService, CodigoPostalDTO } from '../../../services/codigo-postal.service';
import { LanguageService } from '../../../services/language.service';
import { TranslationService } from '../../../services/translation.service';

@Component({
    selector: 'app-codigo-postal-detail',
    templateUrl: './codigo-postal-detail.component.html',
    styleUrls: ['./codigo-postal-detail.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class CodigoPostalDetailComponent implements OnInit, OnDestroy {
  item?: CodigoPostalDTO;
  loading = false;
  error: string | null = null;
  private routeSubscription?: Subscription;
  private languageSubscription?: Subscription;

  constructor(
    private codigoPostalService: CodigoPostalService,
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

    this.codigoPostalService.getById(id).subscribe({
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
