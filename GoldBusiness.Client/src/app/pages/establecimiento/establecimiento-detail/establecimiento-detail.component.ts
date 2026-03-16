import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { EstablecimientoDTO, EstablecimientoService } from '../../../services/establecimiento.service';

@Component({
  selector: 'app-establecimiento-detail',
  templateUrl: './establecimiento-detail.component.html',
  styleUrls: ['./establecimiento-detail.component.css']
})
export class EstablecimientoDetailComponent implements OnInit {
  item?: EstablecimientoDTO;
  loading = false;
  error: string | null = null;

  constructor(
    private establecimientoService: EstablecimientoService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = +params['id'];
      if (id) this.loadItem(id);
    });
  }

  loadItem(id: number): void {
    this.loading = true;
    this.error = null;

    this.establecimientoService.getById(id).subscribe({
      next: item => {
        this.item = item;
        this.loading = false;
      },
      error: err => {
        this.error = err.message || 'Error al cargar el establecimiento';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    // Navigate to the correct list route
    this.router.navigate(['/nomencladores/establecimiento']);
  }
}
