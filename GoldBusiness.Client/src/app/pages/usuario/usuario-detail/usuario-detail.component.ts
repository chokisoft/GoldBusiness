import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { UsuarioDTO, UsuarioService } from '../../../services/usuario.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-usuario-detail',
  templateUrl: './usuario-detail.component.html',
  styleUrls: ['./usuario-detail.component.css']
})
export class UsuarioDetailComponent implements OnInit, OnDestroy {
  item: UsuarioDTO | null = null;
  loading = false;
  error: string | null = null;
  private languageSubscription?: Subscription;

  constructor(
    private route: ActivatedRoute,
    private usuarioService: UsuarioService,
    private router: Router,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadItem(id);
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        const itemId = this.route.snapshot.paramMap.get('id');
        if (itemId) {
          this.loadItem(itemId);
        }
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  loadItem(id: string): void {
    this.loading = true;
    this.error = null;

    this.usuarioService.getById(id).subscribe({
      next: (res) => {
        this.item = res;
        this.loading = false;
      },
      error: (err: Error) => {
        this.error = err.message || 'Error al cargar usuario';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/configuracion/usuarios']);
  }
}
