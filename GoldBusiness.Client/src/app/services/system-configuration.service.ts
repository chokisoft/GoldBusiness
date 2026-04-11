import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { TipoIdentificacionFiscal, RegimenFiscal } from './fiscal.types';

// ═══════════════════════════════════════════════════════════════
// 🔷 INTERFACES
// ═══════════════════════════════════════════════════════════════

export interface Pais {
  id: number;
  descripcion: string;
  regexTelefono?: string;
  formatoTelefono?: string;
  formatoEjemplo?: string;
}

export interface FormaJuridicaDTO {
  id: number;
  descripcion: string;
}

export interface SystemConfigurationDTO {
  id: number;
  codigoSistema: string;
  licencia: string;
  nombreNegocio: string;
  personaContacto?: string;
  formaJuridicaId?: number;
  direccion?: string;
  paisId: number;
  provinciaId: number;
  municipioId: number;
  codigoPostalId: number;
  municipio?: string;
  provincia?: string;
  codPostal?: string;
  imagen?: string;
  web?: string;
  email?: string;
  telefono?: string;
  cuentaPagarId?: number;
  cuentaCobrarId?: number;
  identificadorFiscal?: string;
  tipoIdentificadorFiscal?: TipoIdentificacionFiscal;
  regimenFiscal?: RegimenFiscal;
  tasaIvaDefecto?: number;
  registradaIva?: boolean;
  ivaInternacional?: boolean;
  caducidad: Date;
  creadoPor?: string;
  fechaHoraCreado?: Date;
  modificadoPor?: string;
  fechaHoraModificado?: Date;
  cuentaPagarCodigo?: string;
  cuentaPagarDescripcion?: string;
  cuentaCobrarCodigo?: string;
  cuentaCobrarDescripcion?: string;
  estaVigente?: boolean;
  estaVencida?: boolean;
  proximoAVencer?: boolean;
  diasRestantes?: number;
  estadoLicencia?: string;
  tieneCuentasConfiguradas?: boolean;
  activo: boolean;
  cancelado: boolean;
}

// ═══════════════════════════════════════════════════════════════
// 🔷 SERVICIO
// ═══════════════════════════════════════════════════════════════

@Injectable({
  providedIn: 'root'
})
export class SystemConfigurationService {
  private readonly endpoint = 'SystemConfiguration';

  constructor(private apiService: ApiService) {}

  // ───────────────────────────────────────────────────────────
  // 📖 MÉTODOS DE LECTURA
  // ───────────────────────────────────────────────────────────

  /**
   * Obtiene la configuración del sistema (asume ID = 1)
   */
  get(): Observable<SystemConfigurationDTO> {
    return this.apiService.get<SystemConfigurationDTO>(`${this.endpoint}/1`);
  }

  /**
   * Obtiene todas las configuraciones del sistema
   */
  getAll(): Observable<SystemConfigurationDTO[]> {
    return this.apiService.get<SystemConfigurationDTO[]>(this.endpoint);
  }

  /**
   * Obtiene una configuración específica por ID
   */
  getById(id: number): Observable<SystemConfigurationDTO> {
    return this.apiService.get<SystemConfigurationDTO>(`${this.endpoint}/${id}`);
  }

  /**
   * Obtiene configuraciones paginadas con búsqueda opcional
   */
  getPaged(page: number, pageSize: number, term?: string): Observable<any> {
    let url = `${this.endpoint}/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;
    return this.apiService.get<any>(url);
  }

  // ───────────────────────────────────────────────────────────
  // ✏️ MÉTODOS DE ESCRITURA
  // ───────────────────────────────────────────────────────────

  /**
   * Crea una nueva configuración del sistema
   */
  create(dto: SystemConfigurationDTO): Observable<SystemConfigurationDTO> {
    return this.apiService.post<SystemConfigurationDTO>(this.endpoint, dto);
  }

  /**
   * Actualiza una configuración existente
   * NOTA: El DTO ya contiene el ID, por eso solo recibe 1 parámetro
   */
  update(dto: SystemConfigurationDTO): Observable<SystemConfigurationDTO> {
    return this.apiService.put<SystemConfigurationDTO>(`${this.endpoint}/${dto.id}`, dto);
  }

  /**
   * Elimina (soft delete) una configuración
   */
  delete(id: number): Observable<any> {
    return this.apiService.delete<any>(`${this.endpoint}/${id}`);
  }

  // ───────────────────────────────────────────────────────────
  // 🔍 MÉTODOS AUXILIARES PARA DROPDOWNS
  // ───────────────────────────────────────────────────────────

  /**
   * Obtiene la lista de formas jurídicas para el dropdown
   */
  getFormasJuridicas(lang: string = 'es'): Observable<FormaJuridicaDTO[]> {
    return this.apiService.get<FormaJuridicaDTO[]>(`FormaJuridica?lang=${lang}`);
  }

  /**
   * Obtiene la lista de países para el dropdown
   */
  getPaises(lang: string = 'es'): Observable<Pais[]> {
    return this.apiService.get<Pais[]>(`Pais?lang=${lang}`);
  }

  /**
   * Sube un nuevo logo al servidor
   * NOTA: Los nombres DEBEN coincidir EXACTAMENTE con UploadLogoRequest en el backend (case-sensitive)
   */
  uploadLogo(codigoSistema: string, file: File): Observable<{ fileName: string }> {
    const formData = new FormData();
    // ⭐ IMPORTANTE: Mayúsculas exactas como en C# UploadLogoRequest
    formData.append('file', file); // ⭐ minúscula como antes
    formData.append('codigoSistema', codigoSistema); // ⭐ camelCase como antes
    
    return this.apiService.postFormData<{ fileName: string }>(`${this.endpoint}/upload-logo`, formData);
  }

  /**
   * Construye la URL completa del logo usando el endpoint del backend
   * @param fileName - Nombre del archivo del logo
   * @returns URL completa para mostrar la imagen
   */
  getLogoUrl(fileName: string): string {
    if (!fileName) return '';
    if (fileName.startsWith('http://') || fileName.startsWith('https://')) {
      return fileName;
    }

    const anyApi: any = this.apiService as any;
    if (typeof anyApi.buildUrl === 'function') {
      return anyApi.buildUrl(`${this.endpoint}/logo/${fileName}`);
    }
    return `/api/${this.endpoint}/logo/${fileName}`;
  }

  /**
   * Elimina un logo del servidor
   */
  deleteLogo(fileName: string): Observable<any> {
    return this.apiService.delete<any>(`${this.endpoint}/delete-logo/${fileName}`);
  }
}
