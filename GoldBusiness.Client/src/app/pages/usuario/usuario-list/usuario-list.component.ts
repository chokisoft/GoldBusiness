import { Component, OnDestroy, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { UsuarioDTO, UsuarioService } from '../../../services/usuario.service';
import { LanguageService } from '../../../services/language.service';
import { TranslationService } from '../../../services/translation.service';

@Component({
    selector: 'app-usuario-list',
    templateUrl: './usuario-list.component.html',
    styleUrls: ['./usuario-list.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class UsuarioListComponent implements OnInit, OnDestroy {
  items: UsuarioDTO[] = [];
  loading = false;
  error: string | null = null;
  searchTerm = '';
  searching = false;

  currentPage = 1;
  pageSize = 10;
  totalItems = 0;
  totalPages = 0;
  readonly Math = Math;

  private searchTimer: ReturnType<typeof setTimeout> | null = null;
  private languageSubscription?: Subscription;

  constructor(
    private usuarioService: UsuarioService,
    private router: Router,
    private languageService: LanguageService,
    private translationService: TranslationService
  ) {}

  ngOnInit(): void {
    this.loadData();

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        this.loadData();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
    if (this.searchTimer) {
      clearTimeout(this.searchTimer);
      this.searchTimer = null;
    }
  }

  loadData(): void {
    this.loading = true;
    this.error = null;

    this.usuarioService.getPaged(this.currentPage, this.pageSize, this.searchTerm).subscribe({
      next: (res) => {
        const calculatedTotalPages = Math.ceil(res.total / this.pageSize);

        if (calculatedTotalPages > 0 && this.currentPage > calculatedTotalPages) {
          this.currentPage = calculatedTotalPages;
          this.loadData();
          return;
        }

        this.items = res.items;
        this.totalItems = res.total;
        this.totalPages = calculatedTotalPages;
        if (this.totalPages === 0) {
          this.currentPage = 1;
        }
        this.loading = false;
        this.searching = false;
      },
      error: (err: Error) => {
        this.error = err.message || this.translationService.translate('error.loading');
        this.loading = false;
        this.searching = false;
      }
    });
  }

  onSearch(): void {
    this.searching = true;
    this.currentPage = 1;

    if (this.searchTimer) {
      clearTimeout(this.searchTimer);
    }

    this.searchTimer = setTimeout(() => {
      this.loadData();
    }, 400);
  }

  onPageSizeChange(): void {
    this.currentPage = 1;
    this.loadData();
  }

  getPageNumbers(): number[] {
    if (this.totalPages <= 0) {
      return [];
    }

    const maxButtons = 5;
    let start = Math.max(1, this.currentPage - Math.floor(maxButtons / 2));
    const end = Math.min(this.totalPages, start + maxButtons - 1);

    if ((end - start + 1) < maxButtons) {
      start = Math.max(1, end - maxButtons + 1);
    }

    return Array.from({ length: end - start + 1 }, (_, i) => start + i);
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages || page === this.currentPage) {
      return;
    }

    this.currentPage = page;
    this.loadData();
  }

  previousPage(): void {
    this.goToPage(this.currentPage - 1);
  }

  nextPage(): void {
    this.goToPage(this.currentPage + 1);
  }

  addNew(): void {
    this.router.navigate(['/configuracion/usuarios/nuevo']);
  }

  viewDetail(id: string | undefined): void {
    if (!id) return;
    this.router.navigate(['/configuracion/usuarios', id]);
  }

  edit(id: string | undefined): void {
    if (!id) return;
    this.router.navigate(['/configuracion/usuarios/editar', id]);
  }

  delete(id: string | undefined, name: string): void {
    if (!id) return;

    const confirmDelete = confirm(this.translationService.translate('usuario.confirmDelete', [name]));
    if (!confirmDelete) return;

    this.usuarioService.delete(id).subscribe({
      next: () => this.loadData(),
      error: (err: Error) => this.error = err.message || this.translationService.translate('error.deleting')
    });
  }
}
