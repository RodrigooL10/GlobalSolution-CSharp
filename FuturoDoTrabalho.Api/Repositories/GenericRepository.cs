using Microsoft.EntityFrameworkCore;
using FuturoDoTrabalho.Api.Data;

namespace FuturoDoTrabalho.Api.Repositories
{
    // ====================================================================================
    // REPOSITORY: GENERIC REPOSITORY
    // ====================================================================================
    // Implementação genérica do padrão Repository que fornece operações CRUD básicas
    // para qualquer entidade. Reduz código duplicado e centraliza lógica de acesso a dados.
    // Outros repositories específicos herdam desta classe e adicionam métodos customizados.
    // ====================================================================================
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        // Contexto do Entity Framework para acesso ao banco de dados
        protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        // Buscar uma entidade por ID
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        // Buscar todas as entidades
        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        // Buscar entidades com paginação (útil para listagens grandes)
        public async Task<List<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Set<T>()
                .Skip((pageNumber - 1) * pageSize) // Pular registros das páginas anteriores
                .Take(pageSize) // Pegar apenas o número de registros da página atual
                .ToListAsync();
        }

        // Contar total de entidades (útil para calcular total de páginas)
        public async Task<int> GetCountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        // Criar nova entidade no banco de dados
        public async Task<T> CreateAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // Atualizar entidade existente no banco de dados
        public async Task<T?> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // Deletar entidade por ID
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // Salvar alterações pendentes no banco de dados
        // Útil quando múltiplas operações são feitas antes de salvar
        public async Task<bool> SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
