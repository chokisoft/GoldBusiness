import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { TransaccionService, TransaccionDTO } from '../../../services/transaccion.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-transaccion-detail',
  templateUrl: './transaccion-detail.component.html',
  styleUrls: ['./transaccion-detail.component.css']
})
export class TransaccionDetailComponent implements OnInit, OnDestroy {
  transaccion: TransaccionDTO | null = null;
  id: number | null = null;
  loading = true;
  error: string | null = null;

  private languageSubscription?: Subscription;

  constructor(
    private transaccionService: TransaccionService,
    private route: ActivatedRoute,
    private router: Router,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      this.loadTransaccion();
    } else {
      this.error = 'ID no válido';
      this.loading = false;
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        this.loadTransaccion();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  loadTransaccion(): void {
    if (!this.id) return;

    this.loading = true;
    this.transaccionService.getById(this.id).subscribe({
      next: (data) => {
        this.transaccion = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar el transaccion';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/nomencladores/transaccion']);
  }
}
