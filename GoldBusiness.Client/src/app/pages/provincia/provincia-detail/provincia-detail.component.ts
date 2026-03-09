import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { ProvinciaService, ProvinciaDTO } from '../../../services/provincia.service';

@Component({
  selector: 'app-provincia-detail',
  templateUrl: './provincia-detail.component.html',
  styleUrls: ['./provincia-detail.component.css']
})
export class ProvinciaDetailComponent implements OnInit {
  item?: ProvinciaDTO;
  loading = false;
  error: string | null = null;

  constructor(
    private provinciaService: ProvinciaService,
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

    this.provinciaService.getById(id).subscribe({
      next: (item) => {
        this.item = item;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading provincia:', error);
        this.error = error.message || 'Error al cargar la provincia';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.location.back();
  }
}
