import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { skip, finalize } from 'rxjs/operators';
import { LineaService, LineaDTO } from '../../../services/linea.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-linea-list',
  templateUrl: './linea-list.component.html',
  styleUrls: ['./linea-list.component.css']
})
export class LineaListComponent implements OnInit, OnDestroy {
  lineas: LineaDTO[] = [];
  loading = false;
  searching = false;
  error: string | null = null;

  searchTerm: string = '';
  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;
  totalPages: number = 0;

  Math = Math;

  private languageSubscription?: Subscription;
  private searchDebounceTimer?: any;

  constructor(
    private lineaService: LineaService,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    this.loadData(true);

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 LineaList: Idioma cambiado, recargando...');
        this.loadData(true);
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
    if (this.searchDebounceTimer) clearTimeout(this.searchDebounceTimer);
  }

  loadData(isInitialLoad: boolean = false): void {
    if (isInitialLoad) this.loading = true;
    else this.searching = true;
    
    this.error = null;
    const searchTerm = this.searchTerm.trim() || undefined;
    
    this.lineaService.getPaged(this.currentPage, this.pageSize, searchTerm)
      .pipe(finalize(() => {
        this.loading = false;
        this.searching = false;
      }))
      .subscribe({
        next: (response) => {
          console.log('✅ GET /Linea/paged response:', response);
          this.lineas = response.items;
          this.totalItems = response.total;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
        },
        error: (err) => {
          console.error('❌ Error:', err);
          this.error = `Error al cargar las líneas: ${err?.message ?? err.statusText}`;
        }
      });
  }

  onSearch(): void {
    if (this.searchDebounceTimer) clearTimeout(this.searchDebounceTimer);
    this.searchDebounceTimer = setTimeout(() => {
      this.currentPage = 1;
      this.loadData(false);
    }, 500);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadData(false);
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadData(false);
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadData(false);
    }
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxVisiblePages = 5;
    let startPage = Math.max(1, this.currentPage - Math.floor(maxVisiblePages / 2));
    let endPage = Math.min(this.totalPages, startPage + maxVisiblePages - 1);
    if (endPage - startPage < maxVisiblePages - 1) {
      startPage = Math.max(1, endPage - maxVisiblePages + 1);
    }
    for (let i = startPage; i <= endPage; i++) pages.push(i);
    return pages;
  }

  onPageSizeChange(newSize?: number | string): void {
    const parsed = Number(newSize);
    if (!isNaN(parsed) && parsed > 0) this.pageSize = parsed;
    else if (!this.pageSize || this.pageSize <= 0) this.pageSize = 10;
    this.currentPage = 1;
    this.loadData(false);
  }

  delete(id: number, descripcion: string): void {
    if (confirm(`¿Estás seguro de eliminar la línea "${descripcion}"?`)) {
      this.lineaService.delete(id).subscribe({
        next: () => {
          if (this.lineas.length === 1 && this.currentPage > 1) {
            this.currentPage--;
          }
          this.loadData(false);
        },
        error: (err) => {
          this.error = 'Error al eliminar la línea';
          console.error('Error:', err);
        }
      });
    }
  }
}
