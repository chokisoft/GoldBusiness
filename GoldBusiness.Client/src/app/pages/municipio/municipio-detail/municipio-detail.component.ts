import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { MunicipioService, MunicipioDTO } from '../../../services/municipio.service';
import { LanguageService } from '../../../services/language.service';
import { TranslationService } from '../../../services/translation.service';

@Component({
  selector: 'app-municipio-detail',
  templateUrl: './municipio-detail.component.html',
  styleUrls: ['./municipio-detail.component.css']
})
export class MunicipioDetailComponent implements OnInit, OnDestroy {
  item?: MunicipioDTO;
  loading = false;
  error: string | null = null;
  private routeSubscription?: Subscription;
  private languageSubscription?: Subscription;

  constructor(
    private municipioService: MunicipioService,
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

    this.municipioService.getById(id).subscribe({
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
