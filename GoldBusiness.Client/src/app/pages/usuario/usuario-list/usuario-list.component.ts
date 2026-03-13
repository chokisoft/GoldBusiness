import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UsuarioDTO, UsuarioService } from '../../../services/usuario.service';

@Component({
  selector: 'app-usuario-list',
  templateUrl: './usuario-list.component.html',
  styleUrls: ['./usuario-list.component.css']
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

  constructor(
    private usuarioService: UsuarioService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  ngOnDestroy(): void {
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
        this.items = res.items;
        this.totalItems = res.total;
        this.totalPages = Math.ceil(this.totalItems / this.pageSize);
        this.loading = false;
        this.searching = false;
      },
      error: (err: Error) => {
        this.error = err.message || 'Error al cargar usuarios';
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

    const confirmDelete = confirm(`¿Eliminar el usuario ${name}?`);
    if (!confirmDelete) return;

    this.usuarioService.delete(id).subscribe({
      next: () => this.loadData(),
      error: (err: Error) => this.error = err.message || 'Error al eliminar usuario'
    });
  }
}
