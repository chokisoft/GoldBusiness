import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { CodigoPostalService, CodigoPostalDTO } from '../../../services/codigo-postal.service';

@Component({
  selector: 'app-codigo-postal-detail',
  templateUrl: './codigo-postal-detail.component.html',
  styleUrls: ['./codigo-postal-detail.component.css']
})
export class CodigoPostalDetailComponent implements OnInit {
  item?: CodigoPostalDTO;
  loading = false;
  error: string | null = null;

  constructor(
    private codigoPostalService: CodigoPostalService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = +params['id'];
      if (id) {
        this.loadItem(id);
      }
    });
  }

  loadItem(id: number): void {
    this.loading = true;
    this.error = null;

    this.codigoPostalService.getById(id).subscribe({
      next: (item) => {
        this.item = item;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading código postal:', error);
        this.error = error.message || 'Error al cargar el código postal';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.location.back();
  }
}
