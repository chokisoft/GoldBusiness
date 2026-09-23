import { Component, OnInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { ClienteDTO, ClienteService } from '../../../services/cliente.service';
import { PaisService, PaisDTO } from '../../../services/pais.service';
import { LanguageService } from '../../../services/language.service';
import { TranslationService } from '../../../services/translation.service';

@Component({
    selector: 'app-cliente-detail',
    templateUrl: './cliente-detail.component.html',
    styleUrls: ['./cliente-detail.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class ClienteDetailComponent implements OnInit, OnDestroy {
  item?: ClienteDTO;
  loading = false;
  error: string | null = null;

  selectedPais?: PaisDTO;
  paisDescripcion?: string;
  postalCode?: string;
  private sub?: Subscription;
  private paisSub?: Subscription;
  private languageSubscription?: Subscription;

  constructor(
    private clienteService: ClienteService,
    private route: ActivatedRoute,
    private router: Router,
    private paisService: PaisService,
    private languageService: LanguageService,
    private translationService: TranslationService
  ) {}

  ngOnInit(): void {
    this.sub = this.route.params.subscribe(params => {
      const id = +params['id'];
      if (id) this.loadItem(id);
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
    this.sub?.unsubscribe();
    this.paisSub?.unsubscribe();
    this.languageSubscription?.unsubscribe();
  }

  loadItem(id: number): void {
    this.loading = true;
    this.error = null;
    this.selectedPais = undefined;
    this.paisDescripcion = undefined;
    this.postalCode = undefined;
    this.paisSub?.unsubscribe();

    this.clienteService.getById(id).subscribe({
      next: item => {
        this.item = item;

        // fallbacks from DTO (if backend returned them)
        this.paisDescripcion = (item as any)?.paisDescripcion ?? undefined;
        this.postalCode = (item as any)?.codigoPostalCodigo ?? (item as any)?.codPostal ?? undefined;

        const paisId = (item as any)?.paisId;
        if (paisId) {
          this.paisSub = this.paisService.getById(paisId).subscribe({
            next: p => {
              this.selectedPais = p;
              this.paisDescripcion = p?.descripcion ?? this.paisDescripcion;
            },
            error: () => {
              this.selectedPais = undefined;
            }
          });
        }

        this.loading = false;
      },
      error: err => {
        this.error = err?.message || this.translationService.translate('error.loading');
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/nomencladores/clientes']);
  }
}
