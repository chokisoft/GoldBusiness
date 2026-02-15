import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GrupoCuentaService, GrupoCuentaDTO } from '../../../services/grupo-cuenta.service';

@Component({
  selector: 'app-grupo-cuenta-detail',
  templateUrl: './grupo-cuenta-detail.component.html',
  styleUrls: ['./grupo-cuenta-detail.component.css']
})
export class GrupoCuentaDetailComponent implements OnInit {
  grupo: GrupoCuentaDTO | null = null;
  loading = true;
  error: string | null = null;
  grupoId: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private grupoCuentaService: GrupoCuentaService
  ) {
    this.grupoId = +this.route.snapshot.paramMap.get('id')!;
  }

  ngOnInit(): void {
    this.loadGrupoCuenta();
  }

  loadGrupoCuenta(): void {
    this.loading = true;
    this.error = null;

    this.grupoCuentaService.getById(this.grupoId).subscribe({
      next: (data) => {
        this.grupo = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar el grupo de cuenta';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  goToEdit(): void {
    this.router.navigate(['/nomencladores/grupo-cuenta/editar', this.grupoId]);
  }

  goBack(): void {
    this.router.navigate(['/nomencladores/grupo-cuenta']);
  }

  delete(): void {
    if (!confirm(`¿Está seguro que desea eliminar el grupo "${this.grupo?.nombre}"?`)) {
      return;
    }

    this.grupoCuentaService.delete(this.grupoId).subscribe({
      next: () => {
        this.router.navigate(['/nomencladores/grupo-cuenta']);
      },
      error: (err) => {
        this.error = err.error?.message || 'Error al eliminar el grupo de cuenta';
        console.error('Error:', err);
      }
    });
  }
}
