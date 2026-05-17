using MyPortfolio.Helpers.Interfaces;
using MyPortfolio.Models.Analytics;
using MyPortfolio.Repositories.Interfaces;
using MyPortfolio.Services.Interfaces;
using MyPortfolio.Services.Models;
using MyPortfolio.Services.Models.Analytics;

namespace MyPortfolio.Services.Implementations
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashHelper _hashHelper;

        public AnalyticsService(
            IUnitOfWork unitOfWork,
            IHashHelper hashHelper)
        {
            _unitOfWork = unitOfWork;
            _hashHelper = hashHelper;
        }

        public async Task TrackPageViewAsync(
            string visitorKey,
            string sessionKey,
            string path,
            string? fullUrl,
            string? pageTitle,
            string? referrer,
            string? ipAddress,
            string? userAgent,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(visitorKey))
                return;

            if (string.IsNullOrWhiteSpace(sessionKey))
                return;

            if (ShouldIgnorePath(path))
                return;

            var visitor = await GetOrCreateVisitorAsync(
                visitorKey,
                ipAddress,
                userAgent,
                cancellationToken);

            var session = await GetOrCreateSessionAsync(
                visitor.Id,
                sessionKey,
                ipAddress,
                userAgent,
                referrer,
                cancellationToken);

            var now = DateTime.UtcNow;
            var ipHash = HashIp(ipAddress);

            visitor.LastVisitAt = now;
            visitor.LastIpAddressHash = ipHash;
            visitor.UserAgent = userAgent;

            session.LastActivityAt = now;

            var pageView = new PageView
            {
                VisitorId = visitor.Id,
                VisitorSessionId = session.Id,
                Path = path,
                FullUrl = fullUrl,
                PageTitle = pageTitle,
                Referrer = referrer,
                IpAddressHash = ipHash,
                UserAgent = userAgent,
                CreatedAt = now
            };

            await _unitOfWork.Repository<PageView>().AddAsync(pageView, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<ServiceResult> TrackClickAsync(
            string visitorKey,
            string sessionKey,
            TrackClickRequest request,
            string? ipAddress,
            string? userAgent,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(visitorKey))
                return ServiceResult.Failure("Visitor key is missing.");

            if (string.IsNullOrWhiteSpace(sessionKey))
                return ServiceResult.Failure("Session key is missing.");

            if (string.IsNullOrWhiteSpace(request.EventType))
                return ServiceResult.Failure("Event type is required.");

            if (string.IsNullOrWhiteSpace(request.ComponentName))
                return ServiceResult.Failure("Component name is required.");

            if (string.IsNullOrWhiteSpace(request.PagePath))
                return ServiceResult.Failure("Page path is required.");

            if (ShouldIgnorePath(request.PagePath))
                return ServiceResult.Success();

            var visitor = await GetOrCreateVisitorAsync(
                visitorKey,
                ipAddress,
                userAgent,
                cancellationToken);

            var session = await GetOrCreateSessionAsync(
                visitor.Id,
                sessionKey,
                ipAddress,
                userAgent,
                null,
                cancellationToken);

            var now = DateTime.UtcNow;

            visitor.LastVisitAt = now;
            visitor.LastIpAddressHash = HashIp(ipAddress);
            visitor.UserAgent = userAgent;

            session.LastActivityAt = now;

            var clickEvent = new ClickEvent
            {
                VisitorId = visitor.Id,
                VisitorSessionId = session.Id,
                EventType = request.EventType.Trim(),
                ComponentName = request.ComponentName.Trim(),
                TargetType = request.TargetType?.Trim(),
                TargetId = request.TargetId?.Trim(),
                TargetText = request.TargetText?.Trim(),
                PagePath = request.PagePath.Trim(),
                MetadataJson = request.MetadataJson,
                CreatedAt = now
            };

            await _unitOfWork.Repository<ClickEvent>().AddAsync(clickEvent, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }

        private async Task<Visitor> GetOrCreateVisitorAsync(
            string visitorKey,
            string? ipAddress,
            string? userAgent,
            CancellationToken cancellationToken)
        {
            var visitor = await _unitOfWork.Repository<Visitor>().FirstOrDefaultAsync(
                predicate: x => x.VisitorKey == visitorKey,
                asNoTracking: false,
                cancellationToken: cancellationToken);

            if (visitor is not null)
                return visitor;

            var ipHash = HashIp(ipAddress);
            var now = DateTime.UtcNow;

            visitor = new Visitor
            {
                VisitorKey = visitorKey,
                FirstVisitAt = now,
                LastVisitAt = now,
                FirstIpAddressHash = ipHash,
                LastIpAddressHash = ipHash,
                UserAgent = userAgent
            };

            await _unitOfWork.Repository<Visitor>().AddAsync(visitor, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return visitor;
        }

        private async Task<VisitorSession> GetOrCreateSessionAsync(
            long visitorId,
            string sessionKey,
            string? ipAddress,
            string? userAgent,
            string? referrer,
            CancellationToken cancellationToken)
        {
            var session = await _unitOfWork.Repository<VisitorSession>().FirstOrDefaultAsync(
                predicate: x => x.SessionKey == sessionKey,
                asNoTracking: false,
                cancellationToken: cancellationToken);

            if (session is not null)
                return session;

            var now = DateTime.UtcNow;

            session = new VisitorSession
            {
                VisitorId = visitorId,
                SessionKey = sessionKey,
                StartedAt = now,
                LastActivityAt = now,
                IpAddressHash = HashIp(ipAddress),
                UserAgent = userAgent,
                Referrer = referrer
            };

            await _unitOfWork.Repository<VisitorSession>().AddAsync(session, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return session;
        }

        private string? HashIp(string? ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                return null;

            return _hashHelper.Sha256Hash(ipAddress);
        }

        private static bool ShouldIgnorePath(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return true;

            var normalized = path.ToLowerInvariant();

            return normalized.StartsWith("/admin")
                   || normalized.StartsWith("/css")
                   || normalized.StartsWith("/js")
                   || normalized.StartsWith("/lib")
                   || normalized.StartsWith("/images")
                   || normalized.StartsWith("/uploads")
                   || normalized.StartsWith("/favicon")
                   || normalized.StartsWith("/analytics");
        }
    }
}
