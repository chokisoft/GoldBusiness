import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { skip, finalize } from 'rxjs/operators';
import { SubLineaService, SubLineaDTO } from '../../../services/sub-linea.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-sub-linea-list',
  templateUrl: './sub-linea-list.component.html',
  styleUrls: ['./sub-linea-list.component.css']
})
export class SubLineaListComponent implements OnInit, OnDestroy {
  subLineas: SubLineaDTO[] = [];
  filteredSubLineas: SubLineaDTO[] = [];
  paginatedSubLineas: SubLineaDTO[] = [];
  loading = false;
  error: string | null = null;

  searchTerm: string = '';
  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 0;
  startItem: number = 0;
  endItem: number = 0;
  Math = Math;

  private languageSubscription?: Subscription;

  constructor(
    private subLineaService: SubLineaService,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    this.loadData();

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 SubLineaList: Idioma cambiado, recargando datos...');
        this.loadData();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  loadData(): void {
    this.loading = true;
    this.error = null;

    Promise.resolve().then(() => {
      this.subLineaService.getAll()
        .pipe(finalize(() => this.loading = false))
        .subscribe({
          next: (data) => {
            this.subLineas = data.sort((a, b) => {
              const aVal = a.codigo ?? '';
              const bVal = b.codigo ?? '';
              return aVal.localeCompare(bVal);
            });
            this.filteredSubLineas = [...this.subLineas];

            if (!this.pageSize || this.pageSize <= 0) this.pageSize = 10;
            const totalItems = this.filteredSubLineas.length;
            this.totalPages = totalItems === 0 ? 0 : Math.ceil(totalItems / this.pageSize);

            if (this.totalPages === 0) {
              this.currentPage = 1;
            } else {
              if (this.currentPage < 1) this.currentPage = 1;
              if (this.currentPage > this.totalPages) this.currentPage = this.totalPages;
            }

            this.applyPagination();
          },
          error: (err) => {
            this.error = 'Error al cargar subLineas';
            console.error('Error:', err);
          }
        });
    });
  }

  onSearch(): void {
    const term = this.searchTerm.toLowerCase().trim();

    if (!term) this.filteredSubLineas = [...this.subLineas];
    else {
      this.filteredSubLineas = this.subLineas.filter(item => item.codigo?.toLowerCase().includes(term) ||
        item.descripcion?.toLowerCase().includes(term)
      );
    }

    this.currentPage = 1;
    this.applyPagination();
  }

  applyPagination(): void {
    if (!this.pageSize || this.pageSize <= 0) this.pageSize = 10;

    const totalItems = this.filteredSubLineas.length;
    this.totalPages = totalItems === 0 ? 0 : Math.ceil(totalItems / this.pageSize);

    if (this.totalPages === 0) {
      this.currentPage = 1;
      this.paginatedSubLineas = [];
      this.startItem = 0;
      this.endItem = 0;
      return;
    }

    this.currentPage = Math.min(Math.max(1, this.currentPage), this.totalPages);

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = Math.min(startIndex + this.pageSize, totalItems);

    this.paginatedSubLineas = this.filteredSubLineas.slice(startIndex, endIndex);

    this.startItem = totalItems === 0 ? 0 : startIndex + 1;
    this.endItem = endIndex;
  }

  goToPage(page: number): void {
    if (page >= 1 && (this.totalPages === 0 || page <= this.totalPages)) {
      this.currentPage = page;
      this.applyPagination();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.applyPagination();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.applyPagination();
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
    this.applyPagination();
  }

  delete(id: number, descripcion: string): void {
    if (confirm(`¿Está seguro de eliminar: "${descripcion}"?`)) {
      this.subLineaService.delete(id).subscribe({
        next: () => this.loadData(),
        error: (err) => {
          this.error = 'Error al eliminar el subLinea';
          console.error('Error:', err);
        }
      });
    }
  }
}
